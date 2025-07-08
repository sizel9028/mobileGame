using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 싱글턴 패턴을 사용하는 ActionSystem 클래스
// 한 번에 하나의 GameAction만 실행되며, 각 Action은 Pre → Perform → Post 순으로 실행됨
public class ActionSystem : Singleton<ActionSystem>
{
    // 현재 처리 중인 리액션 리스트 (Pre, Perform, Post 단계마다 바뀜)
    private List<GameAction> reactions = null;

    // 현재 어떤 액션이 실행 중인지 여부를 나타냄
    public bool IsPerforming { get; private set; } = false;

    // Pre 시점에 반응하는 구독자들
    private static Dictionary<Type, List<Action<GameAction>>> preSubs = new();

    // Post 시점에 반응하는 구독자들
    private static Dictionary<Type, List<Action<GameAction>>> postSubs = new();

    // 실제 수행 로직을 담고 있는 performer 딕셔너리
    private static Dictionary<Type, Func<GameAction, IEnumerator>> performers = new();

    // 외부에서 GameAction을 실행하고 싶을 때 호출
    public void Perform(GameAction action, System.Action OnPerformFinished = null)
    {
        // 이미 액션이 실행 중이면 무시
        if (IsPerforming) return;

        IsPerforming = true;

        // 코루틴으로 Flow 시작
        StartCoroutine(Flow(action, () =>
        {
            IsPerforming = false;
            OnPerformFinished?.Invoke(); // 실행이 끝났으면 콜백 호출
        }));
    }

    // 현재 단계의 리액션 리스트에 추가
    public void AddReaction(GameAction gameAction)
    {
        reactions?.Add(gameAction);
    }

    // 액션의 Pre → Perform → Post 단계 흐름을 정의한 메서드
    private IEnumerator Flow(GameAction action, Action OnFlowFinished = null)
    {
        // 1. Pre 단계
        reactions = action.PreReactions;
        PerformSubscribers(action, preSubs); // Pre 구독자 호출
        yield return PerformReactions();     // PreReactions 실행

        // 2. Perform 단계
        reactions = action.PerformReactions;
        yield return PerformPerformer(action); // Performer 실행
        yield return PerformReactions();       // Performer 실행 후 나오는 Reaction들 실행

        // 3. Post 단계
        reactions = action.PostReactions;
        PerformSubscribers(action, postSubs);  // Post 구독자 호출
        yield return PerformReactions();       // PostReactions 실행

        // 모든 흐름이 끝나면 콜백 호출
        OnFlowFinished?.Invoke();
    }

    // Performer 딕셔너리에 등록된 수행자 함수 실행
    private IEnumerator PerformPerformer(GameAction action)
    {
        Type type = action.GetType();
        if (performers.ContainsKey(type))
        {
            // 등록된 performer 실행
            yield return performers[type](action);
        }
    }

    // 구독자 딕셔너리에 따라 반응자들 실행
    private void PerformSubscribers(GameAction action, Dictionary<Type, List<Action<GameAction>>> subs)
    {
        Type type = action.GetType();
        if (subs.ContainsKey(type))
        {
            foreach (var sub in subs[type])
            {
                sub(action); // 각 구독자 함수 실행
            }
        }
    }

    // 현재 리액션 리스트에 있는 액션들을 차례대로 실행
    private IEnumerator PerformReactions()
    {
        foreach (var reaction in reactions)
        {
            yield return Flow(reaction); // 리액션도 똑같이 Flow 처리
        }
    }

    // 특정 GameAction 타입에 대해 performer 등록
    public static void AttachPerformer<T>(Func<T, IEnumerator> performer) where T : GameAction
    {
        Type type = typeof(T);

        // GameAction을 T로 캐스팅해서 실행하는 래핑 함수
        IEnumerator wrappedPerformer(GameAction action) => performer((T)action);

        if (performers.ContainsKey(type))
        {
            performers[type] = wrappedPerformer;
        }
        else
        {
            performers.Add(type, wrappedPerformer);
        }
    }

    // performer 등록 해제
    public static void DetachPerformer<T>() where T : GameAction
    {
        Type type = typeof(T);
        if (performers.ContainsKey(type))
        {
            performers.Remove(type);
        }
    }

    // 특정 GameAction 타입에 대해 Pre/Post 구독자 등록
    public static void SubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        // PRE인지 POST인지에 따라 적절한 딕셔너리 선택
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;

        // GameAction을 T로 캐스팅하는 래핑 함수
        void wrappedReaction(GameAction action) => reaction((T)action);

        if (subs.ContainsKey(typeof(T)))
        {
            subs[typeof(T)].Add(wrappedReaction);
        }
        else
        {
            subs.Add(typeof(T), new());
            subs[typeof(T)].Add(wrappedReaction);
        }
    }

    // 구독자 해제
    public static void UnsubscribeReaction<T>(Action<T> reaction, ReactionTiming timing) where T : GameAction
    {
        Dictionary<Type, List<Action<GameAction>>> subs = timing == ReactionTiming.PRE ? preSubs : postSubs;

        if (subs.ContainsKey(typeof(T)))
        {
            // 이건 문제 있을 수 있음: 래핑된 델리게이트는 새로 만들어지므로 Remove에 실패할 수 있음
            void wrappedReaction(GameAction action) => reaction((T)action);
            subs[typeof(T)].Remove(wrappedReaction);
        }
    }
}

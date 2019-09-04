using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    public static class ExecuteNavigationEvent
    {
        private static List<INavigationEventHandler> s_NavigationEventHandlers = new List<INavigationEventHandler>(10);

        public delegate void EventFunction<THandler>(THandler eventHandler, INavigationParameters parameters) 
            where THandler : INavigationEventHandler;

        private static readonly EventFunction<INavigatingAware> s_NavigatingAwareHandler = Execute;

        private static void Execute(INavigatingAware eventHandler, INavigationParameters navigationParameters)
        {
            eventHandler.OnNavigatingTo(navigationParameters);
        }

        private static readonly EventFunction<INavigatedAware> s_NavigatedAwareHandler = Execute;

        private static void Execute(INavigatedAware eventHandler, INavigationParameters navigationParameters)
        {
            eventHandler.OnNavigatedTo(navigationParameters);
        }

        private static readonly EventFunction<INavigatingAwayAware> s_NavigatingAwayAwareHandler = Execute;

        private static void Execute(INavigatingAwayAware eventHandler, INavigationParameters navigationParameters)
        {
            eventHandler.OnNavigatingAway(navigationParameters);
        }

        private static readonly EventFunction<INavigatingBackAware> s_NavigatingBackAwareHandler = Execute;

        private static void Execute(INavigatingBackAware eventHandler, INavigationParameters navigationParameters)
        {
            eventHandler.OnNavigatingBack(navigationParameters);
        }

        private static readonly EventFunction<INavigatedBackAware> s_NavigatedBackAwareHandler = Execute;
        private static void Execute(INavigatedBackAware eventHandler, INavigationParameters navigationParameters)
        {
            eventHandler.OnNavigatedBack(navigationParameters);
        }

        private static readonly EventFunction<IDestructibleAware> s_DestructibleAwareHandler = Execute;
        private static void Execute(IDestructibleAware eventHandler, INavigationParameters navigationParameters)
        {
            eventHandler.OnScreenDestroyed(navigationParameters);
        }

        public static EventFunction<INavigatingAware> navigatingTo => s_NavigatingAwareHandler;
        public static EventFunction<INavigatedAware> navigatedTo => s_NavigatedAwareHandler;
        public static EventFunction<INavigatingBackAware> navigatingBack => s_NavigatingBackAwareHandler;
        public static EventFunction<INavigatedBackAware> navigatedBack => s_NavigatedBackAwareHandler;
        public static EventFunction<INavigatingAwayAware> navigatingAway => s_NavigatingAwayAwareHandler;
        public static EventFunction<IDestructibleAware> destroyed => s_DestructibleAwareHandler;

        public static void Execute<THandler>(GameObject gameObject,
                                             EventFunction<THandler> function,
                                             INavigationParameters navigationParameters) where THandler: class, INavigationEventHandler
        {
            if (gameObject == null)
                throw new ArgumentNullException(nameof(gameObject));

            if (function == null)
                throw new ArgumentNullException(nameof(function));

            GetHandlersFromGameObject(gameObject, s_NavigationEventHandlers);            
            int count = s_NavigationEventHandlers.Count;
            for(int i = 0; i < count; i++)
            {
                var handler = s_NavigationEventHandlers[i] as THandler;
                if(handler != null)
                    function(handler, navigationParameters);
            }            
        }

        private static void GetHandlersFromGameObject(GameObject gameObject, List<INavigationEventHandler> navigationEvents)
        {
            navigationEvents.Clear();

            gameObject.GetComponentsInChildren(navigationEvents);
        }
    }
}
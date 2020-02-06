using Aci.Unity.Events;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Aci.Unity.UI.Navigation
{
    public sealed class NavigationService : INavigationService
    {
        private static readonly NavigationParameters s_DefaultParams = new NavigationParameters();

        private readonly Stack<IScreenController> m_NavigationStack = new Stack<IScreenController>(25);
        private readonly IScreenRegistry m_ScreenRegistry;
        private readonly IAciEventManager m_EventManager;
        private readonly List<Task> m_ParallelTasks = new List<Task>();

        private IScreenController m_Current;
        private bool m_IsBusy;

        public NavigationService(IScreenRegistry screenRegistry, IAciEventManager aciEventHandler)
        {
            m_ScreenRegistry = screenRegistry;
            m_EventManager = aciEventHandler;
        }

        /// <inheritdoc />
        public bool CanPop()
        {
            return m_NavigationStack.Count > 0;
        }

        /// <inheritdoc />
        public Task PopAsync(AnimationOptions animationOptions)
        {
            s_DefaultParams.Clear();
            return PopAsync(s_DefaultParams, animationOptions);
        }

        /// <inheritdoc />
        public async Task PopAsync(INavigationParameters parameters, AnimationOptions animationOptions)
        {
            if (m_IsBusy)
                return;

            if (m_Current == null)
                throw new InvalidOperationException("Cannot pop if there is no screen current being displayed!");

            IScreenController previousScreen = null;
            if (m_NavigationStack.Count != 0)
                previousScreen = m_NavigationStack.Pop();

            try
            {
                m_IsBusy = true;

                if (!animationOptions.enabled)
                {
                    m_Current.OnScreenDestroyed(parameters);
                    await m_Current.UpdateDisplayAsync(NavigationMode.Removed, false);

                    if (previousScreen != null)
                    {
                        m_Current = previousScreen;
                        m_Current.OnNavigatingBack(parameters);
                        await m_Current.UpdateDisplayAsync(NavigationMode.Returning, false);
                        m_Current.OnNavigatedBack(parameters);
                    }
                    else
                    {
                        m_Current = null;
                    }
                }
                else
                {
                    if (animationOptions.playSynchronously)
                    {
                        m_Current.OnScreenDestroyed(parameters);
                        m_ParallelTasks.Clear();
                        m_ParallelTasks.Add(m_Current.UpdateDisplayAsync(NavigationMode.Removed));
                        if (previousScreen != null)
                        {
                            previousScreen.OnNavigatingBack(parameters);
                            m_ParallelTasks.Add(previousScreen.UpdateDisplayAsync(NavigationMode.Returning));
                        }

                        await Task.WhenAll(m_ParallelTasks);

                        if (previousScreen != null)
                        {
                            m_Current = previousScreen;
                            m_Current.OnNavigatedBack(parameters);
                        }
                        else
                        {
                            m_Current = null;
                        }

                    }
                    else
                    {
                        m_Current.OnScreenDestroyed(parameters);
                        await m_Current.UpdateDisplayAsync(NavigationMode.Removed);
                        m_Current = previousScreen;
                        m_Current.OnNavigatingBack(parameters);
                        await m_Current.UpdateDisplayAsync(NavigationMode.Returning);
                        m_Current.OnNavigatedBack(parameters);
                    }

                    m_EventManager.Invoke(new NavigationCompletedEvent(m_Current.id, m_NavigationStack));
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                m_IsBusy = false;
            }

        }

        public Task PopToRootAsync(AnimationOptions animationOptions)
        {
            s_DefaultParams.Clear();
            return PopToRootAsync(s_DefaultParams, animationOptions);
        }

        public async Task PopToRootAsync(INavigationParameters parameters, AnimationOptions animationOptions)
        {
            if (m_NavigationStack.Count == 0)
                return;

            s_DefaultParams.Clear();
            // Destroy all screens in between
            while (m_NavigationStack.Count > 1)
            {
                var screen = m_NavigationStack.Pop();
                screen.OnScreenDestroyed(s_DefaultParams);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                screen.UpdateDisplayAsync(NavigationMode.Removed, false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

            await PopAsync(parameters, animationOptions);
        }

        /// <inheritdoc />
        public Task PushAsync(string screen, AnimationOptions animationOptions, bool addToHistory = true)
        {
            s_DefaultParams.Clear();
            return PushAsync(screen, s_DefaultParams, animationOptions, addToHistory);
        }

        /// <inheritdoc />
        public async Task PushAsync(string screen, INavigationParameters parameters, AnimationOptions animationOptions, bool addToHistory = true)
        {
            if (m_IsBusy)
                return;

            if (string.IsNullOrEmpty(screen))
                throw new ArgumentNullException(nameof(screen));

            IScreenController nextScreen = null;
            if (!m_ScreenRegistry.TryGetScreen(screen, out nextScreen))
                throw new ArgumentNullException(nameof(screen), $"Screen with id {screen} does not exist!");

            if (m_Current != null && addToHistory)
                m_NavigationStack.Push(m_Current);

            try
            {
                m_IsBusy = true;

                if (!animationOptions.enabled)
                {
                    if (m_Current != null)
                    {
                        m_Current.OnNavigatingAway(parameters);
                        await m_Current.UpdateDisplayAsync(NavigationMode.Leaving, false);

                        if (!addToHistory)
                        {
                            m_Current.OnScreenDestroyed(s_DefaultParams);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            m_Current.UpdateDisplayAsync(NavigationMode.Removed, false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        }
                    }

                    m_Current = nextScreen;
                    m_Current.Prepare();
                    m_Current.OnNavigatingTo(parameters);
                    await m_Current.UpdateDisplayAsync(NavigationMode.Entering, false);
                    m_Current.OnNavigatedTo(parameters);
                }
                else
                {
                    if (animationOptions.playSynchronously)
                    {
                        m_ParallelTasks.Clear();

                        if (m_Current != null)
                        {
                            m_Current.OnNavigatingAway(parameters);
                            m_ParallelTasks.Add(m_Current.UpdateDisplayAsync(NavigationMode.Leaving));
                        }
                        nextScreen.Prepare();
                        nextScreen.OnNavigatingTo(parameters);
                        m_ParallelTasks.Add(nextScreen.UpdateDisplayAsync(NavigationMode.Entering));
                        await Task.WhenAll(m_ParallelTasks);

                        if (m_Current != null && !addToHistory)
                        {
                            m_Current.OnScreenDestroyed(s_DefaultParams);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            m_Current.UpdateDisplayAsync(NavigationMode.Removed, false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        }

                        m_Current = nextScreen;
                        m_Current.OnNavigatedTo(parameters);
                    }
                    else
                    {
                        if (m_Current != null)
                        {
                            m_Current.OnNavigatingAway(parameters);
                            await m_Current.UpdateDisplayAsync(NavigationMode.Leaving);

                            if (!addToHistory)
                            {
                                m_Current.OnScreenDestroyed(s_DefaultParams);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                                m_Current.UpdateDisplayAsync(NavigationMode.Removed, false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                            }
                        }

                        m_Current = nextScreen;
                        m_Current.Prepare();
                        m_Current.OnNavigatingTo(parameters);
                        await m_Current.UpdateDisplayAsync(NavigationMode.Entering);
                        m_Current.OnNavigatedTo(parameters);
                    }
                }

                m_EventManager.Invoke(new NavigationCompletedEvent(m_Current.id, m_NavigationStack));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                m_IsBusy = false;
            }
        }

        /// <inheritdoc />
        public Task PushWithNewStackAsync(string screen, INavigationParameters parameters, AnimationOptions animationOptions)
        {
            if (m_IsBusy)
                return Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(screen))
                throw new ArgumentNullException(nameof(screen));


            if (m_NavigationStack.Count != 0)
            {
                s_DefaultParams.Clear();
                // Destroy all screens in between
                while (m_NavigationStack.Count > 1)
                {
                    IScreenController oldScreen = m_NavigationStack.Pop();
                    oldScreen.OnScreenDestroyed(s_DefaultParams);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    oldScreen.UpdateDisplayAsync(NavigationMode.Removed, false);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }

            return PushAsync(screen, parameters, animationOptions, false);
        }
    }
}
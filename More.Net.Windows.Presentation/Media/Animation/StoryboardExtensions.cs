using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace More.Net.Windows.Media.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public static class StoryboardExtensions
    {
        /// <summary>
        /// Asynchronously animates a storyboard.
        /// </summary>
        /// <typeparam name="TStoryboard"></typeparam>
        /// <param name="storyboard"></param>
        /// <returns></returns>
        public static async Task AnimateAsync<TStoryboard>(
            this TStoryboard storyboard)
            where TStoryboard : Storyboard
        {
            await storyboard
                .AnimateAsync(CancellationToken.None)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously animates a storyboard, supporting cancellation.
        /// </summary>
        /// <typeparam name="TStoryboard"></typeparam>
        /// <param name="storyboard"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task AnimateAsync<TStoryboard>(
            this TStoryboard storyboard, 
            CancellationToken token)
            where TStoryboard : Storyboard
        {
            if (storyboard == null)
                throw new ArgumentNullException("storyboard");

            IObservable<Unit> whenCompleted = Observable
                .FromEventPattern(
                    handler => storyboard.Completed += handler,
                    handler => storyboard.Completed -= handler)
                .Select(_ => Unit.Default)
                .FirstAsync()
                .RunAsync(token);
            storyboard.Begin();
            try
            {
                await whenCompleted;
            }
            catch (InvalidOperationException)
            {

            }
            finally
            {
                storyboard.Stop();
                storyboard.Remove();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStoryboard"></typeparam>
        /// <typeparam name="TTimeline"></typeparam>
        /// <param name="storyboard"></param>
        /// <param name="timeline"></param>
        /// <returns></returns>
        public static TStoryboard AddTimeline<TStoryboard, TTimeline>(
            this TStoryboard storyboard, 
            TTimeline timeline)
            where TStoryboard : Storyboard
            where TTimeline : Timeline
        {
            if (storyboard == null)
                throw new ArgumentNullException("storyboard");

            storyboard.Children.Add(timeline);
            return storyboard;
        }
    }
}

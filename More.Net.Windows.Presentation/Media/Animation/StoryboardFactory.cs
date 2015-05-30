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
    public static class StoryboardFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timelines"></param>
        /// <returns></returns>
        public static Storyboard Create(params Timeline[] timelines)
        {
            Storyboard storyboard = new Storyboard();
            foreach (Timeline timeline in timelines)
                storyboard.AddTimeline(timeline);
            return storyboard;
        }
    }
}

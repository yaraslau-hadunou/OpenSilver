#if WORKINPROGRESS

#if MIGRATION
namespace System.Windows.Media.Animation
#else
namespace Windows.UI.Xaml.Media.Animation
#endif
{
	public sealed partial class QuarticEase : EasingFunctionBase
	{
		protected override double EaseInCore(double normalizedTime)
		{
			return default(double);
		}
	}
}

#endif
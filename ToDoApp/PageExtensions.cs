using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace ToDoApp
{
    public static class PageExtensions
    {
#if IOS || MACCATALYST || WINDOWS
        public static async Task SlideTransition(this Page page, Page newPage)
        {
            var currentPage = page.Parent as VisualElement;
            var navigation = page.Navigation;

            if (currentPage != null && navigation != null)
            {
                var width = currentPage.Width;
                var animation = new Animation();

                // Slide out the current page to the left
                animation.WithConcurrent((f) => currentPage.TranslationX = f, 0, -width, Easing.Linear);

                // Slide in the new page from the right
                newPage.TranslationX = width; // Set initial position of newPage
                animation.WithConcurrent((f) => newPage.TranslationX = f, width, 0, Easing.Linear);

                // Perform the animation
                animation.Commit(page, "SlideTransition", 16, 500, Easing.Linear, (v, c) =>
                {
                    // Navigate to the new page after the animation
                    navigation.PushAsync(newPage);
                });

                // Ensure the new page is added to the navigation stack
                await navigation.PushAsync(newPage, false);
            }
        }
#else
        public static async Task SlideTransition(this Page page, Page newPage)
        {
            await page.Navigation.PushAsync(newPage);
        }
#endif
    }
}

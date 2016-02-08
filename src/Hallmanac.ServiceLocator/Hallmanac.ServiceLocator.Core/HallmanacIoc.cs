using System;


namespace Hallmanac.ServiceLocator.Core
{
    /// <summary>
    /// Class container for managing the IOC container for the application during runtime.
    /// </summary>
    public class HallmanacIoc
    {
        /// <summary>
        /// Static singleton that returns the current IOC container for the application.
        /// </summary>
        public static IServiceLocator Current { get; set; }


        /// <summary>
        /// Sets the service Locator for the appliction. This is typically called during an application startup process of some kind.
        /// </summary>
        public static void SetServiceLocator(Func<IServiceLocator> create)
        {
            if (create == null)
            {
                throw new ArgumentNullException(nameof(create));
            }
            Current = create();
        }
    }
}
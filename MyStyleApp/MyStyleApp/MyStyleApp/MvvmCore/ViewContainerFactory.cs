using Xamarin.Forms;

namespace MvvmCore
{
    public enum ContainerTypeEnum
    {
        MasterDetailContainer,
        NavigationContainer,
        TabbedContainer,
        CarouselContainer
    }

    public class ViewContainerFactory
    {
        public Page GetContainer(ContainerTypeEnum containerType)
        {
            Page page = null;
            switch(containerType)
            {
                case ContainerTypeEnum.MasterDetailContainer:
                    page = new MasterDetailPage();
                    break;
                case ContainerTypeEnum.NavigationContainer:
                    page = new NavigationPage();
                    break;
                case ContainerTypeEnum.TabbedContainer:
                    page = new TabbedPage();
                    break;
                case ContainerTypeEnum.CarouselContainer:
                    page = new CarouselPage();
                    break;
                default:
                    break;
            }
            return page;
        }
    }
}

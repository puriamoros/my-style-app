
using System.Collections.Generic;

namespace XamarinFormsAutofacMvvmStarterKit
{
    public abstract class TabbedViewModelBase: ViewModelBase
    {
        public IList<IViewModel> Children { get; set; }
    }
}

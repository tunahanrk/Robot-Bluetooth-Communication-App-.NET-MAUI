using System.Collections.ObjectModel;

namespace ArslanMobileApp.Pages;

public partial class ShoppingMallsPage : ContentPage
{
    private ObservableCollection<Models.ShoppingMallsClass> malls = new ObservableCollection<Models.ShoppingMallsClass>();
    public ShoppingMallsPage()
	{
		InitializeComponent();
		Malls_CllView.ItemsSource = App.DBTrans.GetMallClasses();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var dbMalls = App.DBTrans.GetMallClasses();
        malls.Clear();
        foreach (var mall in dbMalls)
        {
            malls.Add(mall);
        }
    }
}
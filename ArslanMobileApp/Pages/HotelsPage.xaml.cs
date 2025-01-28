using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace ArslanMobileApp.Pages;

public partial class HotelsPage : ContentPage
{
    private ObservableCollection<Models.HotelsClass> hotels = new ObservableCollection<Models.HotelsClass>();
    public HotelsPage()
    {
        InitializeComponent();
        Htls_CllView.ItemsSource = App.DBTrans.GetHotelsClasses();


    }



    private void Button_Clicked(object sender, EventArgs e)
    {
        var dbHotels = App.DBTrans.GetHotelsClasses();
        hotels.Clear();
        foreach (var hotel in dbHotels)
        {
            hotels.Add(hotel);
        }
    }
}

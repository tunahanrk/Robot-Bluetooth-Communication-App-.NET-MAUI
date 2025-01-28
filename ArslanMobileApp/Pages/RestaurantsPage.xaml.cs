using System.Collections.ObjectModel;

namespace ArslanMobileApp.Pages;

public partial class RestaurantsPage : ContentPage
{
    private ObservableCollection<Models.RestaurantClass> restaurants = new ObservableCollection<Models.RestaurantClass>();
    public RestaurantsPage()
	{
		InitializeComponent();
        Rsts_CllView.ItemsSource = App.DBTrans.GetRestaurantClasses();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        var dbRestaurants = App.DBTrans.GetRestaurantClasses();
        restaurants.Clear();
        foreach (var restaurant in dbRestaurants)
        {
            restaurants.Add(restaurant);
        }
    }
}
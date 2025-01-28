using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using ArslanMobileApp.Models;

namespace ArslanMobileApp
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<BluetoothDeviceInfo> devices = new ObservableCollection<BluetoothDeviceInfo>();
        private BluetoothDeviceInfo selectedDevice;

        public MainPage()
        {
            InitializeComponent();
            DevicesListView.ItemsSource = devices;
        }

        private async void OnDiscoverDevicesClicked(object sender, EventArgs e)
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.Bluetooth>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Bluetooth>();
                }

                if (status == PermissionStatus.Granted)
                {
                    devices.Clear();
                    var bluetoothClient = new BluetoothClient();
                    var discoveredDevices = bluetoothClient.DiscoverDevices();

                    foreach (var device in discoveredDevices)
                    {
                        devices.Add(device);
                    }

                    if (discoveredDevices.Count == 0)
                    {
                        await DisplayAlert("Discovery", "No Bluetooth devices found.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Discovery", "Devices discovered! Select one from the list.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Permission Required", "Bluetooth permission is required to discover devices.", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Bluetooth Discovery Error: {ex}");
                await DisplayAlert("Error", $"An error occurred during discovery: {ex.Message}", "OK");
            }
        }

        private async void OnDeviceSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is BluetoothDeviceInfo device)
            {
                selectedDevice = device;

                try
                {
                    var bluetoothClient = new BluetoothClient();
                    bluetoothClient.Connect(device.DeviceAddress, BluetoothService.SerialPort);

                    await DisplayAlert("Connection Status", $"Connected to {device.DeviceName} ({device.DeviceAddress})", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to connect to {device.DeviceName}: {ex.Message}", "OK");
                }
            }
        }

        private async void OnStartListeningClicked(object sender, EventArgs e)
        {
            if (selectedDevice == null)
            {
                await DisplayAlert("Error", "Please select a device first.", "OK");
                return;
            }

            try
            {
                var bluetoothClient = new BluetoothClient();
                bluetoothClient.Connect(selectedDevice.DeviceAddress, BluetoothService.SerialPort);

                using (var stream = bluetoothClient.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    // Read the command (e.g., "Hotels")
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string command = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Read the JSON data
                    bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string jsonData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    switch (command)
                    {
                        case "Hotels":
                            var hotels = JsonConvert.DeserializeObject<List<HotelsClass>>(jsonData);
                            if (hotels != null && hotels.Any())
                            {
                                App.DBTrans.ClearHotels();
                                App.DBTrans.SaveHotelsToLocalDatabase(hotels, App.DBTrans.dbPath);
                                await DisplayAlert("Data Received", "Hotels data saved successfully.", "OK");
                                await Navigation.PushAsync(new Pages.HotelsPage());
                            }
                            break;

                        case "Restaurants":
                            var restaurants = JsonConvert.DeserializeObject<List<RestaurantClass>>(jsonData);
                            if (restaurants != null && restaurants.Any())
                            {
                                App.DBTrans.ClearRetaurants();
                                App.DBTrans.SaveRestaurantsToLocalDatabase(restaurants, App.DBTrans.dbPath);
                                await DisplayAlert("Data Received", "Restaurants data saved successfully.", "OK");
                                await Navigation.PushAsync(new Pages.RestaurantsPage());
                            }
                            break;

                        case "ShoppingMalls":
                            var malls = JsonConvert.DeserializeObject<List<ShoppingMallsClass>>(jsonData);
                            if (malls != null && malls.Any())
                            {
                                App.DBTrans.ClearMalls();
                                App.DBTrans.SaveMallsToLocalDatabase(malls, App.DBTrans.dbPath);
                                await DisplayAlert("Data Received", "Shopping malls data saved successfully.", "OK");
                                await Navigation.PushAsync(new Pages.ShoppingMallsPage());
                            }
                            break;

                        default:
                            await DisplayAlert("Error", "Unknown data received.", "OK");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
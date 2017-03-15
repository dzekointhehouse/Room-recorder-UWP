using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CodebustersAppWMU3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SurfaceDetails : Page
    {

        public SurfaceDetails()
        {
            this.InitializeComponent();
            ReadFile();
        }
        public async void ReadFile() {
            StorageFolder storageFolder =  await ApplicationData.Current.LocalFolder.CreateFolderAsync("RoomDocumentation",
                    CreationCollisionOption.OpenIfExists); 
           StorageFile ticketsFile = await storageFolder.CreateFileAsync("ElvirrumRight.txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);

            //Write data to the file
            await Windows.Storage.FileIO.WriteTextAsync(ticketsFile, "Right ");

            //read file
            string savedTickets = await Windows.Storage.FileIO.ReadTextAsync(ticketsFile);

            Title.Text = savedTickets;
            //StorageFolder destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("RoomDocumentation",
                    //CreationCollisionOption.OpenIfExists); 
           //// StorageFile sampleFile = await destinationFolder.GetFileAsync("ElvirrumRight.txt");
           // var files = await destinationFolder.GetFilesAsync();
           // var desierdfile = files.FirstOrDefault(x => x.Name == "ElvirrumRighttext.txt");
           // //var text = await Windows.Storage.FileIO.ReadBufferAsync(desierdfile);
           //     string fileContent = await FileIO.ReadTextAsync(desierdfile);

        
           //     fileContent.ToString();
           //     Title.Text = fileContent;
            
        }

}
}

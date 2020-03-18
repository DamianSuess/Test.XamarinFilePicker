using System;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace Test.FilePicker.ViewModels
{
  public class MainPageViewModel : ViewModelBase
  {
    private string _selectedFileName;
    private string _selectedFilePath;
    private ImageSource _selectedFileImage;

    public MainPageViewModel(INavigationService navigationService)
        : base(navigationService)
    {
      Title = "Main Page";
    }

    public string SelectedFileName
    {
      get => _selectedFileName;
      set => SetProperty(ref _selectedFileName, value);
    }

    public string SelectedFilePath
    {
      get => _selectedFilePath;
      set => SetProperty(ref _selectedFilePath, value);
    }

    public DelegateCommand CmdSelectFileAny => new DelegateCommand(OnSelectFileAny);

    public DelegateCommand CmdSelectFileImage => new DelegateCommand(OnSelectFileImage);

    public ImageSource SelectedImage
    {
      get => _selectedFileImage;
      set => SetProperty(ref _selectedFileImage, value);
    }

    private async void OnSelectFileAny()
    {
      var file = await CrossFilePicker.Current.PickFile();
      UpdateSelectedDetails(file);
    }

    private async void OnSelectFileImage()
    {
      string[] fileTypes = null;

      if (Device.RuntimePlatform == Device.Android)
      {
        fileTypes = new string[] { "image/png", "image/jpeg" };
      }

      if (Device.RuntimePlatform == Device.iOS)
      {
        fileTypes = new string[] { "public.image" }; // same as iOS constant UTType.Image
      }

      if (Device.RuntimePlatform == Device.UWP)
      {
        fileTypes = new string[] { ".jpg", ".png" };
      }

      if (Device.RuntimePlatform == Device.WPF)
      {
        fileTypes = new string[] { "JPEG files (*.jpg)|*.jpg", "PNG files (*.png)|*.png" };
      }

      var file = await CrossFilePicker.Current.PickFile(fileTypes);
      UpdateSelectedDetails(file);
    }

    private void UpdateSelectedDetails(FileData file)
    {
      if (file != null)
      {
        SelectedFileName = file.FileName;
        SelectedFilePath = file.FilePath;

        if (file.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase)
          || file.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
        {
          SelectedImage = ImageSource.FromStream(() => file.GetStream());
        }
      }
    }
  }
}

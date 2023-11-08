namespace Project_Manager.App
{
    public class AppHelper
    {
        public static string BaseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "HauntedCrypt", "ProjectManager");
        public static string GetImageFolder()
        {
            var imagePath = Path.Combine(BaseFolder, "Images");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            return imagePath;
        }
    }
}

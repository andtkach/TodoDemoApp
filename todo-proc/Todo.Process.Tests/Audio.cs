namespace Todo.Process.Tests
{
    public class Audio
    {
        [Fact]
        public void ConvertFileToString()
        {
            var bytes = File.ReadAllBytes("c:\\Users\\Andrii\\Downloads\\file_example_MP3_1MG.mp3");
            var base64 = Convert.ToBase64String(bytes);
            File.WriteAllText("c:\\Users\\Andrii\\Downloads\\file_example_MP3_1MG.txt", base64);
        }
    }
}
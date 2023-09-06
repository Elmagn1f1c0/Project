namespace Project.Data.DTO
{
    public class ResponseDTO<T>
    {
        public string DisplayMessage { get; set; }
        public int StatusCode { get; set; }

        public T Result { get; set; }
    }
}

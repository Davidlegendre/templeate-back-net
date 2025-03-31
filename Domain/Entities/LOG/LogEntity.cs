namespace Domain.Entities.LOG
{
    public  class LogEntity
    {
        public long ID {  get; set; }
        public short ID_ENTIDAD { get; set; }
        public short ID_EVENTO { get; set; }
        public string MENSAJE { get; set; }
        public string IDENTIFICADOR { get; set; }
        public DateTime FECHA_REGISTRO { get; set; }

    }
}

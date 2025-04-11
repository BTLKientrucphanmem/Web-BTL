namespace Web_BTL.DataAccessLayer.Models {
    public class Actor_MediaModel {
        public virtual int MediaId { get; set; }
        public virtual MediaModel Media { get; set; }

        public virtual int ActorId { get; set; }
        public virtual ActorModel Actor { get; set; }

        public bool IsMain { get; set; } = false;
    }
}
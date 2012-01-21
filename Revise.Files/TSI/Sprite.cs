
namespace Revise.Files {
    public class Sprite {
        public short Texture {
            get;
            set;
        }

        public int X1 {
            get;
            set;
        }

        public int Y1 {
            get;
            set;
        }

        public int X2 {
            get;
            set;
        }

        public int Y2 {
            get;
            set;
        }

        public int Colour {
            get;
            set;
        }

        public string ID {
            get;
            set;
        }

        public Sprite() {
            ID = string.Empty;
        }
    }
}

using System.ComponentModel;

namespace TetrisUWP
{
    public class GUIData : INotifyPropertyChanged
    {
        private int score;
        private int lines;
        private TetrisLibrary.Shape nextShape;

        public event PropertyChangedEventHandler PropertyChanged;

        public int Score
        {
            get { return score; }
            set
            {
                score = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Score)));
            }
        }

        public int Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lines)));
            }
        }

        public TetrisLibrary.Shape NextShape
        {
            get { return nextShape; }
            set
            {
                nextShape = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NextShape)));
            }
        }

        public GUIData()
        {
            score = 0;
            lines = 0;
            nextShape = null;
        }
    }
}

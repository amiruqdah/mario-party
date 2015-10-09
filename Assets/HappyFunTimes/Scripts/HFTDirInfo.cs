
namespace HappyFunTimes {

  public struct DirInfo {
    public DirInfo(int _direction, float _dx, float _dy, int _bits, string _symbol) {
      direction = _direction;
      dx = _dx;
      dy = _dy;
      bits = _bits;
      symbol = _symbol;
    }
    public int direction;
    public float dx;
    public float dy;
    public int bits;
    public string symbol;
  }

  public class Direction {
    public const int RIGHT = 0x1;
    public const int LEFT  = 0x2;
    public const int UP    = 0x4;
    public const int DOWN  = 0x8;

    public static void Init() {
      s_dirInfo = new DirInfo[9];
      s_dirInfo[0] = new DirInfo(-1,  0,  0, 0           , "\u2751");
      s_dirInfo[1] = new DirInfo( 0,  1,  0, RIGHT       , "\u2192"); // right
      s_dirInfo[2] = new DirInfo( 1,  1,  1, UP | RIGHT  , "\u2197"); // up-right
      s_dirInfo[3] = new DirInfo( 2,  0,  1, UP          , "\u2191"); // up
      s_dirInfo[4] = new DirInfo( 3, -1,  1, UP | LEFT   , "\u2196"); // up-left
      s_dirInfo[5] = new DirInfo( 4, -1,  0, LEFT        , "\u2190"); // left
      s_dirInfo[6] = new DirInfo( 5, -1, -1, DOWN | LEFT , "\u2199"); // down-left
      s_dirInfo[7] = new DirInfo( 6,  0, -1, DOWN        , "\u2193"); // down
      s_dirInfo[8] = new DirInfo( 7,  1, -1, DOWN | RIGHT, "\u2198"); // down-right
    }

    public static DirInfo GetDirectionInfo(int direction) {
        if (s_dirInfo == null) {
          Init();
        }
        return s_dirInfo[direction + 1];
    }

    private static DirInfo[] s_dirInfo = null;
  }

}


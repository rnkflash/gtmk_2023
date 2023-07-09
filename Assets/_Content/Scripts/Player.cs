using System.Threading.Tasks;
using _Content.Scripts.so;
using UnityEngine.SceneManagement;

namespace _Content.Scripts
{
	public class Player : Singleton<Player>
	{

		public Level[] levels = new Level[0];
		public int currentLevel = 0;

		public Level getCurrentLevel()
		{
			if (levels.Length <= 0 || currentLevel >= levels.Length)
				return null;
			return levels[currentLevel];
		}

	}
}
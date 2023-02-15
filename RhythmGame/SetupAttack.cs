using GXPEngine;

public partial class Setup : Game
{
    public void LoadAttack(string key, Unit owner, float atX = -1, float atY = -1)
    {
        #region Position sign
        if (atX == -1) atX = owner.x;
        if (atY == -1) atY = owner.y;
        #endregion

        #region Switching by key
        switch (key)
        {
            case "ExampleCircleAttack":
                ExampleCircleAttack();
                break;
            default:
                break;
        }
        #endregion

        #region Attack Setup
        void ExampleCircleAttack()
        {
            float angle = 0;
            for (int i = 0; i < 16; i++)
            {
                Bullet bullet = ObjectPool<Bullet>.GetInstance(typeof(Bullet)).GetObject(owner);
                if (bullet is null)
                    return;

                bullet.SetXY(x++, atY);
                bullet.rotation = angle;
                angle += 45;
            }
        }
        #endregion
    }
}
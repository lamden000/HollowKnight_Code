using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Object
{
    public class CGameObject
    {
        protected float x;
        protected float y;

        protected float vx;
        protected float vy;

        protected int nx;
        protected int ny;
        protected int state;

        protected bool isDeleted = false;
        protected int type = -1;

        public void SetPosition(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void SetSpeed(float vx, float vy)
        {
            this.vx = vx;
            this.vy = vy;
        }

        public void GetPosition(out float x, out float y)
        {
            x = this.x;
            y = this.y;
        }

        public void GetSpeed(out float vx, out float vy)
        {
            vx = this.vx;
            vy = this.vy;
        }

        public float GetY()
        {
            return y;
        }

        public void SetVy(float vy)
        {
            this.vy = vy;
        }

        public int GetNx()
        {
            return nx;
        }

        public float GetX()
        {
            return x;
        }

        public int GetNy()
        {
            return ny;
        }

        public void SetX(float x)
        {
            this.x = x;
        }

        public void SetType(int type)
        {
            this.type = type;
        }

        public float GetVx()
        {
            return vx;
        }

        public int GetType()
        {
            return type;
        }

        public int GetState()
        {
            return state;
        }

        public virtual void Delete()
        {
            isDeleted = true;
        }

        public bool IsDeleted()
        {
            return isDeleted;
        }

        public CGameObject()
        {
        }

        public CGameObject(float x, float y) : this()
        {
            this.x = x;
            this.y = y;
        }

        public virtual void Update(uint dt, List<CGameObject> coObjects = null) { }

        //public abstract void Render();

        public virtual void SetState(int state)
        {
            this.state = state;
        }

        public static bool IsDeleted(CGameObject obj)
        {
            return obj.isDeleted;
        }
    }
}

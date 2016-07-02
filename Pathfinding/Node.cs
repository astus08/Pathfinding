using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinding
{

    class Node
    {
        private bool blocking;
        private bool start;
        private bool used;
        private bool onPath;
        private bool end;
        private int posX;
        private int posY;
        private int distance;
        private int factor;
        private Node parent;
        private Node left;
        private Node right;
        private Node up;
        private Node down;

        public Node(int x, int y)
        {
            blocking = false;
            start = false;
            end = false;
            used = false;
            onPath = false;
            this.posX = x;
            this.posY = y;
        }

        public bool getPermeability()
        {
            return this.blocking;
        }

        public bool getStart()
        {
            return this.start;
        }

        public bool getEnd()
        {
            return this.end;
        }

        public bool isUsed()
        {
            return this.used;
        }

        public void Use()
        {
            this.used = true;
        }

        public bool isOnPath()
        {
            return this.onPath;
        }

        public void calcDistance(Node target)
        {
            this.distance = Math.Abs(this.posX - target.posX) + Math.Abs(this.posY - target.posY);
        }


        public void setAsClicked()
        {
            if (this.blocking)
            {
                this.reset();
            }
            else
                this.blocking = true;
        }

        public void setAsStart()
        {
            this.blocking = false;
            this.end = false;
            this.start = true;
        }

        public void setAsEnd()
        {
            this.blocking = false;
            this.start = false;
            this.end = true;
        }

        public void reset()
        {
            this.blocking = false;
            this.end = false;
            this.start = false;
            this.used = false;
            this.onPath = false;
        }
    }
}

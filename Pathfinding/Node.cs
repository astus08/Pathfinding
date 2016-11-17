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
            {
                this.blocking = true;
                this.end = false;
                this.start = false;
                this.used = false;
                this.onPath = false;
            }
        }

        public void setAsStart()
        {
            this.blocking = false;
            this.end = false;
            this.start = true;
            this.used = false;
            this.onPath = false;
        }

        public void setAsEnd()
        {
            this.blocking = false;
            this.start = false;
            this.end = true;
            this.used = false;
            this.onPath = false;
        }

        public void setOnPath()
        {
            this.used = false;
            this.blocking = false;
            this.end = false;
            this.start = false;
            this.onPath = true;
        }

        public void use()
        {
            this.used = true;
            this.blocking = false;
            this.end = false;
            this.start = false;
            this.onPath = false;
        }

        public void setDirections(Node l, Node r, Node u, Node d)
        {
            this.left = l;
            this.right = r;
            this.up = u;
            this.down = d;
        }

        public Node getUp()
        {
            return this.up;
        }

        public Node getDown()
        {
            return this.down;
        }

        public Node getLeft()
        {
            return this.left;
        }

        public Node getRight()
        {
            return this.right;
        }

        public void setParent(Node n)
        {
            this.parent = n;
        }

        public Node getParent()
        {
            return this.parent;
        }

        public int getDistance()
        {
            return this.distance;
        }

        public void setFactor(int x)
        {
            this.factor = x;
        }

        public int getFactor()
        {
            return this.factor;
        }

        // When we have find the path, we go on the parent string
        public Node traceBack()
        {
            this.onPath = true;
            return this.parent;
        }

        /// <summary>
        /// Reset the node by switching all the boolean variables to false
        /// </summary>
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

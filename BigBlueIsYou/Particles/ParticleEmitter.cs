using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CS5410
{
    public class ParticleSystem {
        
        private static List<ParticleEmitterLine> particles;
        private static List<ParticleEmitterFromCenter> particles2;
        // private static ParticleEmitterLine m_emitter1;
        // private static ParticleEmitterLine m_emitter2;
        private static ParticleEmitterFromCenter m_emitter3;

        public ParticleSystem(){
            
            particles = new List<ParticleEmitterLine>();
            particles2 = new List<ParticleEmitterFromCenter>();

        }
        public static void update(GameTime gameTime) {
            
            updateParticles(gameTime);
        }
        public static void render(SpriteBatch m_spriteBatch) {
            
            drawParticles(m_spriteBatch);
        }
        // public void emitSparklesYou(){
        //     foreach(Thing y in you){
        //         makePartilesAroundThing(y.X, y.Y);
        //     }
        // }
        // public void emitSparklesWin(){
        //     foreach(Thing w in win){
        //         makePartilesAroundThing(w.X, w.Y);
        //     }
        // }
        public static void drawParticles(SpriteBatch spriteBatch){
            if(particles.Count > 0){
                foreach(ParticleEmitterLine pe in particles){ 
                    pe.draw(spriteBatch);
                }
            }
            if(particles2.Count > 0){
                foreach(ParticleEmitterFromCenter pe in particles2){ 
                    pe.draw(spriteBatch);
                }
            }

        }
        public static void updateParticles(GameTime gameTime){
            foreach(ParticleEmitterLine pe in particles){ // foreach loop going through list of emmiters and updating them then I need to draw the particles with a render funciton
                pe.update(gameTime);
            }
            foreach(ParticleEmitterFromCenter pe in particles2){
                pe.update(gameTime);
            }
        }
                
        public static void makePartilesAroundThing(int x, int y, int spritesize, Texture2D fire){
            for (int i = 0; i < spritesize; i+= 2){
                Console.WriteLine("This happens");
                PartilesAroundThing((x+2) * spritesize + i, (y+2)* spritesize, fire);
            }
            for (int i = 0; i < spritesize; i+= 2){
                Console.WriteLine("This happens");
                PartilesAroundThing((x+2) * spritesize + i, (y+2)* spritesize + spritesize, fire);
            }
            for (int i = 0; i < spritesize; i+= 2){
                Console.WriteLine("This happens");
                PartilesAroundThing((x+2) * spritesize, (y+2)* spritesize + i, fire);
            }
            for (int i = 0; i < spritesize; i+= 2){
                Console.WriteLine("This happens");
                PartilesAroundThing((x+2) * spritesize + spritesize, (y+2)* spritesize + i, fire);
            }
        }
                
        public static void PartilesAroundThing(int x, int y, Texture2D fire){
            // Console.WriteLine("Death!");
            m_emitter3 = new ParticleEmitterFromCenter(
                fire,
                new TimeSpan(0, 0, 0, 0, 75),
                x, y,
                7,
                1,
                new TimeSpan(0, 0, 0, 0, 150));
            particles2.Add(m_emitter3);
        }
        public static void makeDeathPartiles(int x, int y, Texture2D fire){
            // Console.WriteLine("Death!");
            m_emitter3 = new ParticleEmitterFromCenter(
                fire,
                new TimeSpan(0, 0, 0, 0, 5),
                x, y,
                15,
                1,
                new TimeSpan(0, 0, 0, 0, 150));
            particles2.Add(m_emitter3);
        }
        public static void endParticle() {
            particles.Clear();
            particles2.Clear();
        }


    }
    public class ParticleEmitterLine
    {

        private Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        private Texture2D m_texFire;
        private MyRandom m_random = new MyRandom();

        private TimeSpan m_rate;
        private int m_sourceX;
        private int m_sourceY;
        private int m_sarticleSize;
        private int m_speed;
        private TimeSpan m_lifetime;

        public Vector2 Gravity { get; set; }

        public ParticleEmitterLine(Texture2D fire, TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            m_rate = rate;
            m_sourceX = sourceX;
            m_sourceY = sourceY;
            m_sarticleSize = size;
            m_speed = speed;
            m_lifetime = lifetime;
            m_texFire = fire;

            // m_texSmoke = content.Load<Texture2D>("Images/smoke");
            // m_texFire = content.Load<Texture2D>("Images/fire");

            this.Gravity = new Vector2(0, 0);
        }

        private TimeSpan m_accumulated = TimeSpan.Zero;

        /// <summary>
        /// Generates new particles, updates the state of existing ones and retires expired particles.
        /// </summary>
        public void update(GameTime gameTime)
        {
            //
            // Generate particles at the specified rate
            m_accumulated += gameTime.ElapsedGameTime;
            while (m_accumulated > m_rate)
            {
                m_accumulated -= m_rate;

                Particle p = new Particle(
                    m_random.Next(),
                    new Vector2(m_sourceX, m_sourceY),
                    new Vector2(0, -1),
                    (float)m_random.nextGaussian(m_speed, 1),
                    m_lifetime);

                if (!m_particles.ContainsKey(p.name))
                {
                    m_particles.Add(p.name, p);
                }
            }


            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in m_particles.Values)
            {
                p.lifetime -= gameTime.ElapsedGameTime;
                if (p.lifetime < TimeSpan.Zero)
                {
                    //
                    // Add to the remove list
                    removeMe.Add(p.name);
                }
                //
                // Update its position
                p.position += (p.direction * p.speed);
                //
                // Have it rotate proportional to its speed
                p.rotation += p.speed / 50.0f;
                //
                // Apply some gravity
                p.direction += this.Gravity;
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(0, 0, m_sarticleSize, m_sarticleSize);
            foreach (Particle p in m_particles.Values)
            {
                Texture2D texDraw;
                texDraw = m_texFire;

                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                spriteBatch.Draw(
                    texDraw,
                    r,
                    null,
                    Color.White,
                    p.rotation,
                    new Vector2(texDraw.Width / 2, texDraw.Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }
    }
    public class ParticleEmitterFromCenter
    {

        private Dictionary<int, Particle> m_particles = new Dictionary<int, Particle>();
        private Texture2D m_texFire;
        private MyRandom m_random = new MyRandom();

        private TimeSpan m_rate;
        private int m_sourceX;
        private int m_sourceY;
        private int m_sarticleSize;
        private int m_speed;
        private TimeSpan m_lifetime;

        public Vector2 Gravity { get; set; }

        public ParticleEmitterFromCenter(Texture2D fire, TimeSpan rate, int sourceX, int sourceY, int size, int speed, TimeSpan lifetime)
        {
            m_rate = rate;
            m_sourceX = sourceX;
            m_sourceY = sourceY;
            m_sarticleSize = size;
            m_speed = speed;
            m_lifetime = lifetime;
            m_texFire = fire;

            // m_texSmoke = content.Load<Texture2D>("Images/smoke");
            // m_texFire = content.Load<Texture2D>("Images/fire");

            this.Gravity = new Vector2(0, 0);
        }

        private TimeSpan m_accumulated = TimeSpan.Zero;

        /// <summary>
        /// Generates new particles, updates the state of existing ones and retires expired particles.
        /// </summary>
        public void update(GameTime gameTime)
        {
            //
            // Generate particles at the specified rate
            m_accumulated += gameTime.ElapsedGameTime;
            while (m_accumulated > m_rate)
            {
                m_accumulated -= m_rate;

                Particle p = new Particle(
                    m_random.Next(),
                    new Vector2(m_sourceX, m_sourceY),
                    m_random.nextCircleVector(),
                    (float)m_random.nextGaussian(m_speed, 1),
                    m_lifetime);

                if (!m_particles.ContainsKey(p.name))
                {
                    m_particles.Add(p.name, p);
                }
            }


            //
            // For any existing particles, update them, if we find ones that have expired, add them
            // to the remove list.
            List<int> removeMe = new List<int>();
            foreach (Particle p in m_particles.Values)
            {
                p.lifetime -= gameTime.ElapsedGameTime;
                if (p.lifetime < TimeSpan.Zero)
                {
                    //
                    // Add to the remove list
                    removeMe.Add(p.name);
                }
                //
                // Update its position
                p.position += (p.direction * p.speed);
                //
                // Have it rotate proportional to its speed
                p.rotation += p.speed / 50.0f;
                //
                // Apply some gravity
                p.direction += this.Gravity;
            }

            //
            // Remove any expired particles
            foreach (int Key in removeMe)
            {
                m_particles.Remove(Key);
            }
        }

        /// <summary>
        /// Renders the active particles
        /// </summary>
        public void draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle(0, 0, m_sarticleSize, m_sarticleSize);
            foreach (Particle p in m_particles.Values)
            {
                Texture2D texDraw;
                texDraw = m_texFire;

                r.X = (int)p.position.X;
                r.Y = (int)p.position.Y;
                spriteBatch.Draw(
                    texDraw,
                    r,
                    null,
                    Color.White,
                    p.rotation,
                    new Vector2(texDraw.Width / 2, texDraw.Height / 2),
                    SpriteEffects.None,
                    0);
            }
        }
    }
}

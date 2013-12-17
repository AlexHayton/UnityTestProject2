using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TestProject;
using UnityEngine;
using System.Collections;

public class DrainableContainer : MonoBehaviour
{
    public float MinDistBetweenHoles;
    public float HoleRadius = .1f;
    public bool InfiniteSupply = false;

    public ParticleSystem LeakEffect;

    private float contentsZeroY;
    private float contentsOriginalHeight;
    private float contentsOriginalMaxY;
    private Vector3 contentsOriginalScale;
    private Vector3 contentsOriginalPosition;
    private GameObject contents;
    private float contentsCurrentMaxY;
    private float contentsCurrentHeight;
    private float contentsCrossSection;
    private float gravity;
    private List<PunctureHole> holes;
    // Use this for initialization

    public class PunctureHole
    {
        public ParticleSystem Particles;
        public Vector3 Position;
        public float Radius;
        public DrainableContainer Container;
        public float TimeStopped { get; private set; }

        public PunctureHole(Vector3 position, Vector3 direction, float radius, DrainableContainer container)
        {
            Container = container;
            Position = position;
            Radius = radius;
            Particles = (ParticleSystem)Instantiate(Container.LeakEffect, position, Quaternion.LookRotation(direction));
            Particles.Play();
            TimeStopped = -1;
        }
        public float Leak()
        {
            var deltaH = Container.contentsCurrentMaxY - Position.y;
            var velocity = Mathf.Sqrt(2 * Container.gravity * deltaH);
            var flowAmount = velocity * Radius * Radius * Mathf.PI;
            Particles.startSpeed = velocity;
            //Particles.emissionRate = flowAmount;
            return flowAmount * Time.deltaTime;
        }

        public void StopLeaking()
        {
            if (Particles.isPlaying)
            {
                Particles.Stop();
                TimeStopped = Time.time;
            }
        }
    }

    void Start()
    {
        MinDistBetweenHoles *= MinDistBetweenHoles;

        holes = new List<PunctureHole>();

        contents = transform.FindChild("Contents").gameObject;
        contentsOriginalPosition = contents.transform.position;
        contentsZeroY = contents.renderer.bounds.min.y;
        contentsOriginalMaxY = contents.renderer.bounds.max.y;
        contentsCurrentMaxY = contentsOriginalMaxY;
        contentsOriginalHeight = contentsOriginalMaxY - contentsZeroY;
        contentsCurrentHeight = contentsOriginalHeight;
        contentsOriginalScale = contents.transform.localScale;
        contentsCrossSection = (contents.renderer.bounds.center - contents.renderer.bounds.max).sqrMagnitude;
        gravity = Physics.gravity.magnitude;
    }

    public void AddHole(Vector3 position, Vector3 normal, float radius = -1)
    {
        if (radius < 0)
            radius = HoleRadius;
        if (!holes.Any(a => (a.Position - position).sqrMagnitude < MinDistBetweenHoles))
        {
            var holeToAdd = new PunctureHole(position, -normal, radius, this);
            holes.Add(holeToAdd);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
            AddHole(collision.contacts[0].point, collision.contacts[0].normal);
    }

    // Update is called once per frame
    private void Update()
    {
        if (holes.Any() && !InfiniteSupply)
        {
            var holesToLeakFrom = holes.Where(a => a.Position.y < contentsCurrentMaxY).ToList();
            var totalAmountLost = holesToLeakFrom.Sum(punctureHole => punctureHole.Leak());

            for (int i = 0; i < holes.Count; i++)
            {
                if (holes[i].Position.y > contentsCurrentMaxY)
                {
                    holes[i].StopLeaking();
                }
                if (holes[i].TimeStopped > 0 && Time.time - holes[i].TimeStopped > holes[i].Particles.startLifetime)
                {
                    Destroy(holes[i].Particles);
                    holes.RemoveAt(i);
                }

            }
            var deltaH = totalAmountLost / contentsCrossSection;
            var newHeight = contentsCurrentHeight - deltaH;
            var newScale = contentsOriginalScale.z * newHeight / contentsOriginalHeight;
            contents.transform.localScale = new Vector3(contents.transform.localScale.x, newScale, contents.transform.localScale.z);
            contents.transform.position -= new Vector3(0, deltaH / 2, 0);
            contentsCurrentMaxY = contents.renderer.bounds.max.y;
            contentsCurrentHeight = contentsCurrentMaxY - contents.renderer.bounds.min.y;
        }
    }
}

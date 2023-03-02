using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Autohand.Demo {
    public class FireExtinguisher : MonoBehaviour {
        public Grabbable handle;
        public Grabbable hose;
        public Grabbable pin;
        public Transform firePosition;
        [Range(0.1f, 10f)]
        public float shotPower = 5.0f;
        public ConfigurableJoint pinJoint;
        public float explosionDelay = 2;
        public bool startDelayOnRelease = false;
        public float explosionRadius = 10;
        public float pinJointStrength = 750f;
        public GameObject smokeEffect;
        public UnityEvent pinBreakEvent;
        public UnityEvent explosionEvent;
        private bool isPinOff = false;
        private bool isLaberOn = false;

        private void OnEnable() {
            pin.isGrabbable = false;
            handle.OnGrabEvent += OnhandleGrab;
            handle.OnReleaseEvent += OnhandleRelease;
            pin.OnGrabEvent += OnPinGrab;
            pin.OnReleaseEvent += OnPinRelease;
            if(!handle.jointedBodies.Contains(pin.body))
                handle.jointedBodies.Add(pin.body);
            if(!pin.jointedBodies.Contains(handle.body))
                pin.jointedBodies.Add(handle.body);
        }

        private void OnDisable() {
            handle.OnGrabEvent -= OnhandleGrab;
            handle.OnReleaseEvent -= OnhandleRelease;
            pin.OnGrabEvent -= OnPinGrab;
            pin.OnReleaseEvent -= OnPinRelease;
        }

        void OnhandleGrab(Hand hand, Grabbable grab) {
            if(pinJoint != null) {
                pin.isGrabbable = true;
            }
        }

        void OnhandleRelease(Hand hand, Grabbable grab) {
            if(pinJoint != null) {
                pin.isGrabbable = false;
            }
            if(handle != null && startDelayOnRelease)
                Invoke("CheckJointBreak", explosionDelay + Time.fixedDeltaTime * 3);

        }
        void OnPinGrab(Hand hand, Grabbable grab) {
            if(pinJoint != null) {
                pinJoint.breakForce = pinJointStrength;
            }
        }

        void OnPinRelease(Hand hand, Grabbable grab) {
            if(pinJoint != null) {
                pinJoint.breakForce = 100000;
            }

        }

        private void OnJointBreak(float breakForce) {
            Invoke("CheckJointBreak", Time.fixedDeltaTime*2);
        }

        void CheckJointBreak() {
            if(pinJoint == null) {
                pin.maintainGrabOffset = false;
                pin.RemoveJointedBody(handle.body);
                handle.RemoveJointedBody(pin.body);
                if(!startDelayOnRelease)
                    isPinOff = true;
            }
        }

        private void Update(){
            if(isPinOff && isLaberOn)
                Shot();
        }
        
        void Shot() {
            /*
            var hits = Physics.OverlapSphere(handle.transform.position, explosionRadius);
            foreach(var hit in hits) {
                if(AutoHandPlayer.Instance.body == hit.attachedRigidbody) {
                    AutoHandPlayer.Instance.DisableGrounding(0.05f);
                    var dist = Vector3.Distance(hit.attachedRigidbody.position, handle.transform.position);
                    explosionForce *= 2;
                    hit.attachedRigidbody.AddExplosionForce(explosionForce - explosionForce * (dist / explosionRadius), handle.transform.position, explosionRadius);
                    explosionForce /= 2;
                }
                if(hit.attachedRigidbody != null) {
                    var dist = Vector3.Distance(hit.attachedRigidbody.position, handle.transform.position);
                    hit.attachedRigidbody.AddExplosionForce(explosionForce - explosionForce * (dist / explosionRadius), handle.transform.position, explosionRadius);
                }
            }
            explosionEvent?.Invoke();
            */
            Rigidbody smoke = GameObject.Instantiate(smokeEffect, firePosition.position, hose.transform.rotation).GetComponent<Rigidbody>();
            smoke.AddForce((firePosition.position - hose.transform.position) * shotPower * 500, ForceMode.Force);
            GameObject.Destroy(smoke.gameObject, 1.5f);
        }

        public void OnSqueeze()
        {
            isLaberOn = true;
        }

        public void OnUnSqueeze()
        {
            isLaberOn = false;
        }


        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.red;
            if(handle != null)
            Gizmos.DrawWireSphere(handle.transform.position, explosionRadius);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class CameraHandler : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerManager playerManager;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        Transform myTransform;

        Vector3 cameraTransformPosition;
        public LayerMask ignoreLayers;
        Vector3 cameraFollowVelocity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.03f;

        float targetPosition;
        float defaultPosition;
        float lookAngle;
        float pivotAngle;

        public float minimumPivot = -35f;
        public float maximumPivot = 35f;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float lockedPivotPosition = 2.25f;
        public float unlockedPivotPosition = 1.15f;

        public CharacterManager currentLockOnTarget;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public CharacterManager nearestLockOnTarget;
        public CharacterManager leftLockTarget;
        public CharacterManager rightLockTarget;
        public float maximumLockOnDistance = 30f;


        void Awake()
        {
            singleton = this;

            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            playerManager = FindObjectOfType<PlayerManager>();
            targetTransform = playerManager.transform;
            inputHandler = FindObjectOfType<InputHandler>();
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;

            HandleCameraCollision(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (inputHandler.inventoryFlag) { return; }
            if (!inputHandler.lockOnFlag && currentLockOnTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                // lookAngle += mouseXInput * lookSpeed * delta;
                // pivotAngle -= mouseYInput * pivotSpeed * delta;

                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivot, maximumPivot);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;
                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                Vector3 dir = (currentLockOnTarget.transform.position - transform.position).normalized;
                dir.y = 0f;

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = (currentLockOnTarget.transform.position - cameraPivotTransform.position).normalized;
                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        void HandleCameraCollision(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dis = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dis - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;

            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 20f);

            for (int i = 0; i < colliders.Length; i++)
            {
                EnemyManager character = colliders[i].GetComponent<EnemyManager>();
                EnemyStats characterStats = colliders[i].GetComponent<EnemyStats>();

                if (character != null)
                {
                    Vector3 lockTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraTransform.forward);

                    RaycastHit hit;

                    if (character.transform.root != targetTransform.transform.root
                        && viewableAngle > -50 && viewableAngle < 50 && distanceFromTarget <= maximumLockOnDistance)
                    {
                        if (Physics.Linecast(playerManager.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            if (hit.transform.gameObject.tag == "Enemy")
                            {
                                if (!characterStats.isDead)
                                {
                                    availableTargets.Add(character);
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < availableTargets.Count; i++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[i].transform.position);
                float shortestDistanceOfLeftTarget = Mathf.Infinity;
                float shortestDistanceOfRightTarget = -Mathf.Infinity;

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[i];
                }

                if (inputHandler.lockOnFlag)
                {
                    // Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[i].transform.position);
                    // float distanceFromLeftTarget = currentLockOnTarget.transform.position.x - availableTargets[i].transform.position.x;
                    // float distanceFromRightTarget = currentLockOnTarget.transform.position.x + availableTargets[i].transform.position.x;
                    Vector3 relativeEnemyPosition = currentLockOnTarget.transform.InverseTransformPoint(availableTargets[i].transform.position);
                    float distanceFromLeftTarget = relativeEnemyPosition.x;
                    float distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x >= 0f && distanceFromLeftTarget < shortestDistanceOfLeftTarget &&
                        availableTargets[i] != currentLockOnTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        leftLockTarget = availableTargets[i];
                    }
                    else if (relativeEnemyPosition.x <= 0f && distanceFromRightTarget > shortestDistanceOfRightTarget &&
                        availableTargets[i] != currentLockOnTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        rightLockTarget = availableTargets[i];
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            availableTargets.Clear();
            inputHandler.lockOnFlag = false;
            nearestLockOnTarget = null;
            currentLockOnTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;

            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnLockedPosition = new Vector3(0, unlockedPivotPosition);

            if (currentLockOnTarget != null)
            {
                cameraPivotTransform.transform.localPosition = newUnLockedPosition;
                // cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.transform.localPosition = newLockedPosition;

                // cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newUnLockedPosition, ref velocity, Time.deltaTime);
            }
        }
    }
}

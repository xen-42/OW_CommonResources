﻿using OWML.Utils;
using PacificEngine.OW_CommonResources.Game.Display;
using PacificEngine.OW_CommonResources.Game.Resource;
using PacificEngine.OW_CommonResources.Geometry;
using PacificEngine.OW_CommonResources.Geometry.Orbits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_CommonResources.Game.State
{
    public class ScaleState
    {
        public static ScaleState identity { get { return new ScaleState(Vector3.one, Vector3.one); } }

        public Vector3 lossyScale { get; }
        public Vector3 localScale { get; }

        public ScaleState(Vector3 lossyScale, Vector3 localScale)
        {
            this.lossyScale = lossyScale;
            this.localScale = localScale;
        }

        public static ScaleState fromCurrentState(Position.HeavenlyBodies target)
        {
            return fromCurrentState(Position.getBody(target));
        }

        public static ScaleState fromCurrentState(OWRigidbody target)
        {
            if (target == null || target.GetRigidbody() == null)
            {
                return null;
            }

            var lossyScale = target?.transform?.lossyScale ?? identity.lossyScale;
            var localScale = target.GetLocalScale();

            return new ScaleState(lossyScale, localScale);
        }

        public override string ToString()
        {
            var lossyScale = this.lossyScale == null ? "" : DisplayConsole.logVector(this.lossyScale);
            var localScale = this.localScale == null ? "" : DisplayConsole.logVector(this.localScale);
            return $"({lossyScale}, {localScale})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is ScaleState)
            {
                var obj = other as ScaleState;
                return lossyScale == obj.lossyScale
                   && localScale == obj.localScale;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (lossyScale.GetHashCode() * 4)
                + (localScale.GetHashCode() * 16);
        }
    }

    public class PositionState
    {
        public static PositionState identity { get { return new PositionState(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero); } }

        public Vector3 position { get; }
        public Vector3 velocity { get; }
        public Vector3 acceleration { get; }
        public Vector3 jerk { get; }

        public PositionState(Vector3 position, Vector3 velocity, Vector3 acceleration, Vector3 jerk)
        {
            this.position = position;
            this.velocity = velocity;
            this.acceleration = acceleration;
            this.jerk = jerk;
        }

        public static PositionState fromCurrentState(Position.HeavenlyBodies target)
        {
            return fromCurrentState(Position.getBody(target));
        }

        public static PositionState fromCurrentState(OWRigidbody target)
        {
            if (target == null || target.GetRigidbody() == null)
            {
                return null;
            }

            var position = target.GetWorldCenterOfMass() - (Locator.GetCenterOfTheUniverse()?.GetOffsetPosition() ?? Vector3.zero);
            var velocity = target.GetVelocity() - (Locator.GetCenterOfTheUniverse()?.GetOffsetVelocity() ?? Vector3.zero);
            var acceleration = target.GetAcceleration();
            var jerk = target.GetJerk();

            return new PositionState(position, velocity, acceleration, jerk);
        }

        public override string ToString()
        {
            var position = this.position == null ? "" : DisplayConsole.logVector(this.position);
            var velocity = this.velocity == null ? "" : DisplayConsole.logVector(this.velocity);
            var acceleration = this.acceleration == null ? "" : DisplayConsole.logVector(this.acceleration);
            var jerk = this.jerk == null ? "" : DisplayConsole.logVector(this.jerk);
            return $"({position}, {velocity}, {acceleration}, {jerk})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is PositionState)
            {
                var obj = other as PositionState;
                return position == obj.position
                   && velocity == obj.velocity
                   && acceleration == obj.acceleration
                   && jerk == obj.jerk;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (position.GetHashCode() * 4)
                + (velocity.GetHashCode() * 16)
                + (acceleration.GetHashCode() * 64)
                + (acceleration.GetHashCode() * 256);
        }
    }

    public class OrientationState
    {
        public static OrientationState identity { get { return new OrientationState(Quaternion.identity, Vector3.zero, Vector3.zero); } }

        public Quaternion rotation { get; }
        public Vector3 angularVelocity { get; }
        public Vector3 angularAcceleration { get; }

        public OrientationState(Quaternion rotation, Vector3 angularVelocity, Vector3 angularAcceleration)
        {
            this.rotation = rotation;
            this.angularVelocity = angularVelocity;
            this.angularAcceleration = angularAcceleration;
        }

        public static OrientationState fromCurrentState(Position.HeavenlyBodies target)
        {
            return fromCurrentState(Position.getBody(target));
        }

        public static OrientationState fromCurrentState(OWRigidbody target)
        {
            if (target == null || target.GetRigidbody() == null)
            {
                return null;
            }

            var rotation = target.GetRotation();
            var angularVelocity = target.GetAngularVelocity();
            var angularAcceleration = target.GetAngularAcceleration();

            return new OrientationState(rotation, angularVelocity, angularAcceleration);
        }

        public override string ToString()
        {
            var orientation = this.rotation == null ? "" : DisplayConsole.logQuaternion(this.rotation);
            var angularVelocity = this.angularVelocity == null ? "" : DisplayConsole.logVector(this.angularVelocity);
            var angularAcceleration = this.angularAcceleration == null ? "" : DisplayConsole.logVector(this.angularAcceleration);
            return $"({orientation}, {angularVelocity}, {angularAcceleration})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is OrientationState)
            {
                var obj = other as OrientationState;
                return rotation == obj.rotation
                    && angularVelocity == obj.angularVelocity
                    && angularAcceleration == obj.angularAcceleration;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (rotation.GetHashCode() * 4)
                + (angularVelocity.GetHashCode() * 16)
                + (angularAcceleration.GetHashCode() * 64);
        }
    }

    public class KeplerState
    {
        public ScaleState scale { get; }
        public KeplerCoordinates coordinates { get; }
        public OrientationState orientation { get; }

        public Vector3 lossyScale { get { return scale?.lossyScale ?? ScaleState.identity.lossyScale; } }
        public Vector3 localScale { get { return scale?.localScale ?? ScaleState.identity.localScale; } }

        public Quaternion rotation { get { return orientation?.rotation ?? OrientationState.identity.rotation; } }
        public Vector3 angularVelocity { get { return orientation?.angularVelocity ?? OrientationState.identity.angularVelocity; } }
        public Vector3 angularAcceleration { get { return orientation?.angularAcceleration ?? OrientationState.identity.angularAcceleration; } }

        public KeplerState(ScaleState scale, KeplerCoordinates coordinates, OrientationState orientation)
        {
            this.scale = scale ?? ScaleState.identity;
            this.coordinates = coordinates;
            this.orientation = orientation ?? OrientationState.identity;
        }

        public override string ToString()
        {
            return $"({scale}, {coordinates}, {orientation})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is KeplerState)
            {
                var obj = other as KeplerState;
                return scale == obj.scale
                    && coordinates == obj.coordinates
                    && orientation == obj.orientation;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (scale.GetHashCode())
                + (coordinates.GetHashCode() * 4)
                + (orientation.GetHashCode() * 16);
        }
    }

    public class MovementState
    {
        public ScaleState scale { get; }
        public PositionState coordinates { get; }
        public OrientationState orientation { get; }

        public Vector3 lossyScale { get { return scale?.lossyScale ?? ScaleState.identity.lossyScale; } }
        public Vector3 localScale { get { return scale?.localScale ?? ScaleState.identity.localScale; } }

        public Vector3 position { get { return coordinates?.position ?? PositionState.identity.position; } }
        public Vector3 velocity { get { return coordinates?.velocity ?? PositionState.identity.velocity; } }
        public Vector3 acceleration { get { return coordinates?.acceleration ?? PositionState.identity.acceleration; } }
        public Vector3 jerk { get { return coordinates?.jerk ?? PositionState.identity.jerk; } }

        public Quaternion rotation { get { return orientation?.rotation ?? OrientationState.identity.rotation; } }
        public Vector3 angularVelocity { get { return orientation?.angularVelocity ?? OrientationState.identity.angularVelocity; } }
        public Vector3 angularAcceleration { get { return orientation?.angularAcceleration ?? OrientationState.identity.angularAcceleration; } }

        public MovementState(ScaleState scale, PositionState coordinates, OrientationState orientation)
        {
            this.scale = scale ?? ScaleState.identity;
            this.coordinates = coordinates ?? PositionState.identity;
            this.orientation = orientation ?? OrientationState.identity;
        }

        public override string ToString()
        {
            return $"({scale}, {coordinates}, {orientation})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is MovementState)
            {
                var obj = other as MovementState;
                return scale == obj.scale
                    && coordinates == obj.coordinates
                    && orientation == obj.orientation;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (scale.GetHashCode())
                + (coordinates.GetHashCode() * 4)
                + (orientation.GetHashCode() * 16);
        }
    }

    public class AbsoluteState : MovementState
    {
        public AbsoluteState(ScaleState scale, PositionState coordinates, OrientationState orientation) : base(scale, coordinates, orientation)
        {
        }

        public static AbsoluteState fromCurrentState(Position.HeavenlyBodies target)
        {
            return fromCurrentState(Position.getBody(target));
        }

        public static AbsoluteState fromCurrentState(OWRigidbody target)
        {
            if (target == null || target.GetRigidbody() == null)
            {
                return null;
            }
            return new AbsoluteState(ScaleState.fromCurrentState(target), PositionState.fromCurrentState(target), OrientationState.fromCurrentState(target));
        }

        public override string ToString()
        {
            return $"({scale}, {coordinates}, {orientation})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is AbsoluteState)
            {
                var obj = other as AbsoluteState;
                return scale == obj.scale
                    && coordinates == obj.coordinates
                    && orientation == obj.orientation;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (scale.GetHashCode())
                + (coordinates.GetHashCode() * 4)
                + (orientation.GetHashCode() * 16);
        }

        public Vector3 GetPointVelocity(Vector3 worldPoint) => this.GetPointTangentialVelocity(worldPoint) + velocity;

        public Vector3 GetPointTangentialVelocity(Vector3 worldPoint) => Vector3.Cross(angularVelocity, worldPoint - position);

        public Vector3 GetPointAcceleration(Vector3 worldPoint)
        {
            var rhs = worldPoint - position;
            return acceleration + Vector3.Cross(angularVelocity, Vector3.Cross(angularVelocity, rhs)) + Vector3.Cross(angularAcceleration, rhs);
        }

        public Vector3 GetPointTangentialAcceleration(Vector3 worldPoint)
        {
            var rhs = worldPoint - position;
            return Vector3.Cross(angularAcceleration, rhs);
        }

        public Vector3 GetPointCentripetalAcceleration(Vector3 worldPoint)
        {
            return Vector3.Cross(angularVelocity, Vector3.Cross(angularVelocity, worldPoint - position));
        }

        public Vector3 TransformDirection(Vector3 localDirection)
        {
            return rotation * localDirection;
        }

        public Vector3 InverseTransformDirection(Vector3 worldDirection)
        {
            return Quaternion.Inverse(rotation) * worldDirection;
        }

        public Vector3 TransformPoint(Vector3 localPoint)
        {
            return position + rotation * Vector3.Scale(lossyScale, localPoint);
        }

        public Vector3 InverseTransformPoint(Vector3 worldPoint)
        {
            return Vector3.Scale(new Vector3(1 / lossyScale.x, 1 / lossyScale.y, 1 / lossyScale.z),
               Quaternion.Inverse(rotation) * (worldPoint - position));
        }

        public Quaternion TransformRotation(Quaternion localRotation)
        {
            return localRotation * rotation;
        }

        public Quaternion InverseTransformRotation(Quaternion worldRotation)
        {
            return Quaternion.Inverse(rotation) * worldRotation;
        }

        public void apply(OWRigidbody target)
        {
            applyMovement(target);
            applyRotation(target);
        }

        public void apply(Position.HeavenlyBodies parent, AbsoluteState parentState, OWRigidbody target)
        {
            applyMovement(target);
            applyRotation(target);
            applyCachedState(parent, parentState, target);
        }

        private void applyMovement(OWRigidbody target)
        {
            Helper.helper.Console.WriteLine($"{target}: {position}, {velocity}, {acceleration}");

            var p = position + (Locator.GetCenterOfTheUniverse()?.GetOffsetPosition() ?? Vector3.zero);
            var v = velocity + (Locator.GetCenterOfTheUniverse()?.GetOffsetVelocity() ?? Vector3.zero); /* Possibly not required because of CenterOfTheUniverseOffsetApplier */

            target.SetPosition(new Vector3(p.x, p.y, p.z));
            target.SetVelocity(new Vector3(v.x, v.y, v.z));

            target.SetValue("_lastPosition", new Vector3(p.x, p.y, p.z));
            target.SetValue("_currentVelocity", new Vector3(v.x, v.y, v.z));
            target.SetValue("_lastVelocity", new Vector3(v.x, v.y, v.z));
            target.SetValue("_currentAccel", new Vector3(acceleration.x, acceleration.y, acceleration.z));
            target.SetValue("_lastAccel", new Vector3(acceleration.x, acceleration.y, acceleration.z));
        }

        private void applyRotation(OWRigidbody target)
        {
            target.SetRotation(new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w));
            target.SetAngularVelocity(new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z));

            target.SetValue("_currentAngularVelocity", new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z));
            target.SetValue("_lastAngularVelocity", new Vector3(angularVelocity.x, angularVelocity.y, angularVelocity.z));
        }

        private void applyCachedState(Position.HeavenlyBodies parent, AbsoluteState parentState, OWRigidbody target)
        {
            if (target.IsSuspended())
            {
                var parentBody = Position.getBody(parent);
                target.SetValue("_suspensionBody", parentBody);
                target.SetValue("_cachedRelativeVelocity", (parentBody == null || parentState == null) ? Vector3.zero : parentState.InverseTransformDirection(velocity - parentState.GetPointVelocity(position)));
                target.SetValue("_cachedAngularVelocity", angularVelocity);
            }
        }
    }

    public class RelativeState
    {
        public Position.HeavenlyBodies parent { get; }
        public MovementState relative { get; }
        public MovementState surface { get; }
        public KeplerState orbit { get; }


        private RelativeState(Position.HeavenlyBodies parent, MovementState relative, MovementState surface, KeplerState orbit)
        {
            this.parent = parent;
            this.relative = relative;
            this.surface = surface;
            this.orbit = orbit;
        }

        public override string ToString()
        {
            var orbit = (this.orbit?.ToString() ?? "");
            return $"({parent}, {relative}, {surface}, {orbit})";
        }

        public override bool Equals(System.Object other)
        {
            if (other != null && other is RelativeState)
            {
                var obj = other as RelativeState;
                return parent == obj.parent
                    && relative == obj.relative
                    && surface == obj.surface
                    && orbit == obj.orbit;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (parent.GetHashCode())
                + (relative.GetHashCode() >> 2)
                + (surface.GetHashCode() >> 4)
                + (orbit.GetHashCode() >> 6);
        }

        public AbsoluteState apply(OWRigidbody body)
        {
            var parentState = AbsoluteState.fromCurrentState(parent);
            var parentGravity = Position.getGravity(parent);

            return apply(body, parentState, parentGravity);
        }

        public AbsoluteState apply(OWRigidbody body, AbsoluteState parentState, Gravity gravity)
        {
            var state = getAbsoluteState(parentState, gravity);
            if (state != null)
            {
                state.apply(parent, parentState, body);
            }
            return state;
        }

        public AbsoluteState getAbsoluteState()
        {
            var parentState = AbsoluteState.fromCurrentState(parent);
            var parentGravity = Position.getGravity(parent);

            return getAbsoluteState(parentState, parentGravity);
        }

        public AbsoluteState getAbsoluteState(AbsoluteState parentState, Gravity gravity)
        {
            var scale = getAbsoluteScale(parentState, gravity);
            if (scale == null)
            {
                return null;
            }

            var movement = getAbsoluteMovement(parentState, gravity);
            if (movement == null)
            {
                return null;
            }

            var orientation = getAbsoluteOrientation(parentState, gravity, movement.position);
            if (orientation == null)
            {
                return null;
            }

            return new AbsoluteState(scale, movement, orientation);
        }


        private ScaleState getAbsoluteScale(AbsoluteState parentState, Gravity gravity)
        {
            if (orbit != null && orbit.coordinates != null && orbit.coordinates.isOrbit() && gravity != null)
            {
                return orbit.scale;
            }
            else if (parentState != null
                && surface != null)
            {
                return surface.scale;
            }
            else if (relative != null)
            {
                return relative.scale;
            }

            return null;
        }

        private PositionState getAbsoluteMovement(AbsoluteState parentState, Gravity gravity)
        {
            if (orbit != null && orbit.coordinates != null && orbit.coordinates.isOrbit() && gravity != null)
            {
                return getAbsoluteFromKeplerState(parentState, gravity);
            }
            else if (parentState != null
                && surface != null)
            {
                return getAbsoluteFromSurfaceState(parentState);
            }
            else if (relative != null)
            {
                return getAbsoluteFromRelativeState(parentState);
            }

            return null;
        }

        private PositionState getAbsoluteFromKeplerState(AbsoluteState parentState, Gravity gravity)
        {
            var cartesian = OrbitHelper.toCartesian(gravity, Time.timeSinceLevelLoad, orbit.coordinates);
            var position = cartesian.Item1;
            var velocity = cartesian.Item2;
            var acceleration = Vector3.zero;
            var jerk = Vector3.zero;

            Helper.helper.Console.WriteLine($"{orbit.coordinates} -> {position}, {velocity} -> {OrbitHelper.toKeplerCoordinates(gravity, Time.timeSinceLevelLoad, position, velocity)}");

            if (parentState != null && surface != null)
            {
                position = surface.position.normalized * position.magnitude;
                position = parentState.TransformPoint(position);
                velocity += parentState.velocity;
                // TODO: Fix velocity to point in correct direction
            }
            else if (parentState != null)
            {
                position += parentState.position;
                velocity += parentState.velocity;
            }

            Helper.helper.Console.WriteLine($"{orbit.coordinates} -> {position}, {velocity} -> {Position.getKepler(parentState, gravity, position, velocity)}");

            return new PositionState(position, velocity, acceleration, jerk);
        }

        private PositionState getAbsoluteFromSurfaceState(AbsoluteState parentState)
        {
            var position = this.surface.position;
            var velocity = this.surface.velocity;
            var acceleration = this.surface.acceleration;
            var jerk = this.surface.jerk;

            position = parentState.TransformPoint(position);
            velocity += parentState.GetPointVelocity(position);
            acceleration += parentState.GetPointAcceleration(position);

            return new PositionState(position, velocity, acceleration, jerk);
        }

        private PositionState getAbsoluteFromRelativeState(AbsoluteState parentState)
        {
            var position = this.relative.position;
            var velocity = this.relative.velocity;
            var acceleration = this.relative.acceleration;
            var jerk = this.relative.jerk;

            if (parentState != null)
            {
                position += parentState.position;
                velocity += parentState.velocity;
                acceleration += parentState.acceleration;
            }

            return new PositionState(position, velocity, acceleration, jerk);
        }

        private OrientationState getAbsoluteOrientation(AbsoluteState parentState, Gravity gravity, Vector3 worldPosition)
        {
            if (orbit != null && orbit.coordinates != null && orbit.coordinates.isOrbit() && gravity != null)
            {
                var rotation = orbit.rotation; // TODO
                var velocity = orbit.angularVelocity; // TODO

                return new OrientationState(rotation, velocity, Vector3.zero);
            }
            if (parentState != null && surface != null)
            {
                var rotation = parentState.TransformRotation(surface.rotation);
                var velocity = surface.angularVelocity; // TODO

                return new OrientationState(rotation, velocity, Vector3.zero);
            }
            else if (relative != null)
            {
                var rotation = relative.rotation;
                var velocity = relative.angularVelocity;

                return new OrientationState(rotation, velocity, Vector3.zero);
            }

            return new OrientationState(Quaternion.identity, Vector3.zero, Vector3.zero);
        }

        public static RelativeState fromGlobal(Position.HeavenlyBodies parent, OWRigidbody target)
        {
            return fromGlobal(parent, AbsoluteState.fromCurrentState(Position.getBody(parent)), Position.getGravity(parent), Position.getSize(parent), ScaleState.fromCurrentState(target), AbsoluteState.fromCurrentState(target));
        }

        public static RelativeState fromGlobal(Position.HeavenlyBodies parent, AbsoluteState parentState, Gravity parentGravity, Position.Size parentSize, ScaleState targetScale, AbsoluteState target)
        {
            MovementState surfaceMovement = null;
            KeplerState orbit = null;

            parent = parentState == null ? Position.HeavenlyBodies.None : parent;
            var relativeMovement = getRelativeMovement(parentState, parentGravity, targetScale, target);
            if (relativeMovement == null)
            {
                return null;
            }
            else if (parentState != null && parentSize != null)
            {
                var distance = relativeMovement.position.sqrMagnitude;
                if (distance < (parentSize.influence * parentSize.influence))
                {
                    surfaceMovement = getSurfaceMovement(parentState, parentGravity, targetScale, target);
                }

                if ((parentSize.size * parentSize.size) < distance && distance < (parentSize.influence * parentSize.influence))
                {
                    var kepler = getKepler(parentState, parentGravity, targetScale, target);
                    if (kepler != null && kepler.coordinates != null && kepler.coordinates.isOrbit())
                    {
                        if (parentSize.size < kepler.coordinates.perigee && kepler.coordinates.apogee < parentSize.influence)
                        {
                            orbit = kepler;
                        }
                    }
                }
            }

            return new RelativeState(parent, relativeMovement, surfaceMovement, orbit);
        }

        public static MovementState getRelativeMovement(Position.HeavenlyBodies parent, OWRigidbody target)
        {
            return getRelativeMovement(AbsoluteState.fromCurrentState(Position.getBody(parent)), Position.getGravity(parent), ScaleState.fromCurrentState(target), AbsoluteState.fromCurrentState(target));
        }

        public static MovementState getRelativeMovement(AbsoluteState parentState, Gravity parentGravity, ScaleState targetScale, AbsoluteState target)
        {
            if (target == null)
            {
                return null;
            }

            var relativePosition = target.position - (parentState == null ? (Locator.GetCenterOfTheUniverse()?.GetOffsetPosition() ?? Vector3.zero) : parentState.position);
            var relativeVelocity = target.velocity - (parentState == null ? (Locator.GetCenterOfTheUniverse()?.GetOffsetVelocity() ?? Vector3.zero) : parentState.velocity);
            var relativeAcceleration = target.acceleration - (parentState == null ? Vector3.zero : parentState.acceleration);
            var relativeJerk = target.jerk - (parentState == null ? Vector3.zero : parentState.jerk);
            var relativeOrientation = target.rotation;
            var relaitveAngularVelocity = target.velocity;
            var relativeAngularAcceleration = target.acceleration;

            return new MovementState(targetScale, new PositionState(relativePosition, relativeVelocity, relativeAcceleration, relativeJerk), new OrientationState(relativeOrientation, relaitveAngularVelocity, relativeAngularAcceleration));
        }

        public static MovementState getSurfaceMovement(Position.HeavenlyBodies parent, OWRigidbody target)
        {
            return getSurfaceMovement(AbsoluteState.fromCurrentState(Position.getBody(parent)), Position.getGravity(parent), ScaleState.fromCurrentState(target), AbsoluteState.fromCurrentState(target));
        }

        public static MovementState getSurfaceMovement(AbsoluteState parentState, Gravity parentGravity, ScaleState targetScale, AbsoluteState target)
        {
            if (parentState == null || target == null)
            {
                return null;
            }

            var surfacePosition = parentState.InverseTransformPoint(target.position);
            var surfaceVelocity = target.velocity - parentState.GetPointVelocity(target.position);
            var surfaceAcceleration = target.acceleration - parentState.GetPointAcceleration(target.position);
            var surfaceJerk = target.jerk - (parentState == null ? Vector3.zero : parentState.jerk);
            var surfraceOrientation = parentState.InverseTransformRotation(target.rotation);
            var surfaceAngularVelocity = Vector3.zero; // TODO
            var surfaceAngularAcceleration = Vector3.zero; // TODO

            return new MovementState(targetScale, new PositionState(surfacePosition, surfaceVelocity, surfaceAcceleration, surfaceJerk), new OrientationState(surfraceOrientation, surfaceAngularVelocity, surfaceAngularAcceleration));
        }

        public static KeplerState getKepler(Position.HeavenlyBodies parent, OWRigidbody target)
        {
            return getKepler(AbsoluteState.fromCurrentState(Position.getBody(parent)), Position.getGravity(parent), ScaleState.fromCurrentState(target), AbsoluteState.fromCurrentState(target));
        }

        public static KeplerState getKepler(AbsoluteState parentState, Gravity parentGravity, ScaleState targetScale, AbsoluteState target)
        {
            if (parentState == null || parentGravity == null || target == null)
            {
                return null;
            }

            return new KeplerState(targetScale, Position.getKepler(parentState, parentGravity, target.position, target.velocity), new OrientationState(target.rotation, target.angularVelocity, target.angularAcceleration));
        }

        public static RelativeState fromRelative(Position.HeavenlyBodies parent, MovementState relative)
        {
            if (relative == null)
            {
                return null;
            }
            return new RelativeState(parent, relative, null, null);
        }

        public static RelativeState fromSurface(Position.HeavenlyBodies parent, MovementState surface)
        {
            if (surface == null)
            {
                return null;
            }
            return new RelativeState(parent, null, surface, null);
        }

        public static RelativeState fromKepler(Position.HeavenlyBodies parent, ScaleState targetScale, KeplerCoordinates kepler, OrientationState orientation)
        {
            if (kepler != null && kepler.isOrbit())
            {
                return new RelativeState(parent, null, null, new KeplerState(targetScale, kepler, orientation));
            }
            return null;
        }

        public static RelativeState fromClosetInfluence(OWRigidbody target, params Position.HeavenlyBodies[] exclude)
        {
            var targetState = PositionState.fromCurrentState(target);
            if (targetState == null)
            {
                return null;
            }

            var includes = (Position.HeavenlyBodies[])Enum.GetValues(typeof(Position.HeavenlyBodies));
            var excl = new HashSet<Position.HeavenlyBodies>(exclude);
            var parent = Position.getClosest(targetState.position, (body) =>
            {
                if (excl.Contains(body))
                {
                    return true;
                }

                var parentState = PositionState.fromCurrentState(body);
                var size = Position.getSize(body);
                if (parentState == null || size == null)
                {
                    return false;
                }
                else if ((targetState.position - parentState.position).sqrMagnitude < size.influence * size.influence)
                {
                    return false;
                }
                return true;
            }, includes);

            if (parent.Count < 1)
            {
                return null;
            }
            return fromGlobal(parent[0].Item1, target);
        }
    }
}

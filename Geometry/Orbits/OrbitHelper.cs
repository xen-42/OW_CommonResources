﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PacificEngine.OW_CommonResources.Geometry.Orbits
{
    public static class OrbitHelper
    {

        //https://space.stackexchange.com/questions/19322/converting-orbital-elements-to-cartesian-state-vectors
        //https://web.archive.org/web/20160418175843/https://ccar.colorado.edu/asen5070/handouts/cart2kep2002.pdf
        //https://downloads.rene-schwarz.com/download/M002-Cartesian_State_Vectors_to_Keplerian_Orbit_Elements.pdf
        /*public static KeplerCoordinates toKeplerCoordinates(Gravity parent, float timeSinceStart, Vector3 startPosition, Vector3 startVelocity)
        {
            startPosition = transform(startPosition);
            startVelocity = transform(startVelocity);
            var product = Vector3.Dot(startPosition, startVelocity);
            var orbitalMomentum = Vector3.Cross(startPosition, startVelocity);
            var angularMomemntum = (double)orbitalMomentum.magnitude;

            var mu = parent.mu;
            var radius = (double)startPosition.magnitude;
            var speed = (double)startVelocity.magnitude;
            double specificEnergy;
            double semiMajorRadius;
            double eccentricity;

            if (parent.exponent < 1.5d)
            {
                specificEnergy = (speed * speed) / 2d - mu;
                eccentricity = Math.Abs(Math.Sqrt(Math.Abs(((2d * specificEnergy) / mu) + 2d)) - 1d); // TODO: Incorrect for exponent 1
                //semiMajorRadius = radius * Math.Abs(mu / (2d * specificEnergy));
                semiMajorRadius = angularMomemntum / (Math.Sqrt(mu) * (1d - (eccentricity * eccentricity)));
            }
            else
            {
                //https://web.archive.org/web/20160418175843/https://ccar.colorado.edu/asen5070/handouts/cart2kep2002.pdf
                specificEnergy = (speed * speed) / 2d - (mu / radius);
                semiMajorRadius = Math.Abs(mu / (2d * specificEnergy));
                //eccentricity = Math.Sqrt(Math.Abs(1 - ((angularMomemntum * angularMomemntum) / (semiMajorRadius * mu))));
                eccentricity = Math.Sqrt(Math.Abs(1d + (2d * specificEnergy * angularMomemntum * angularMomemntum) / (mu * mu)));
            }

            // https://downloads.rene-schwarz.com/download/M002-Cartesian_State_Vectors_to_Keplerian_Orbit_Elements.pdf
            var inclinationAngle = Math.Acos(orbitalMomentum.z / angularMomemntum);
            var eccentricityVector = (float)eccentricity * ((Vector3.Cross(startVelocity, orbitalMomentum) / mu) - startPosition.normalized).normalized;
            var pointingVector = Vector3.Cross(new Vector3(0, 0, 1), new Vector3(-1f * orbitalMomentum.y, orbitalMomentum.x, 0));
            var trueAnomaly = product < 0f
                ? ((2f * Math.PI) - Math.Acos(Vector3.Dot(eccentricityVector, startPosition) / (startPosition.magnitude * eccentricity)))
                : Math.Acos(Vector3.Dot(eccentricityVector, startPosition) / (startPosition.magnitude * eccentricity));
            var ascendingAngle = pointingVector.y < 0f
                ? ((2f * Math.PI) - Math.Acos(pointingVector.x / pointingVector.magnitude))
                : Math.Acos(pointingVector.x / pointingVector.magnitude);
            var periapseAngle = eccentricityVector.z < 0f 
                ? ((2f * Math.PI) - Math.Acos(Vector3.Dot(pointingVector, eccentricityVector) / (pointingVector.magnitude * eccentricity)))
                : Math.Acos(Vector3.Dot(pointingVector, eccentricityVector) / (pointingVector.magnitude * eccentricity));

            var kepler = KeplerCoordinates.fromTrueAnomaly((float)eccentricity, (float)semiMajorRadius, Angle.toDegrees((float)inclinationAngle), Angle.toDegrees((float)periapseAngle), Angle.toDegrees((float)ascendingAngle), Angle.toDegrees((float)trueAnomaly));

            return KeplerCoordinates.fromTimeSincePeriapsis(parent, (float)eccentricity, (float)semiMajorRadius, Angle.toDegrees((float)inclinationAngle), Angle.toDegrees((float)periapseAngle), Angle.toDegrees((float)ascendingAngle), kepler.getTimeSincePeriapsis(parent) - timeSinceStart);
        }*/

        //https://space.stackexchange.com/questions/19322/converting-orbital-elements-to-cartesian-state-vectors
        //https://web.archive.org/web/20160418175843/https://ccar.colorado.edu/asen5070/handouts/cart2kep2002.pdf
        //https://downloads.rene-schwarz.com/download/M002-Cartesian_State_Vectors_to_Keplerian_Orbit_Elements.pdf
        public static KeplerCoordinates toKeplerCoordinates(Gravity parent, float timeSinceStart, Vector3 startPosition, Vector3 startVelocity)
        {
            startPosition = transform(startPosition);
            startVelocity = transform(startVelocity);
            var product = Vector3.Dot(startPosition, startVelocity);
            var orbitalMomentum = Vector3.Cross(startPosition, startVelocity);
            var angularMomemntum = (double)orbitalMomentum.magnitude;

            var mu = parent.mu;
            var radius = (double)startPosition.magnitude;
            var speed = (double)startVelocity.magnitude;
            double specificEnergy;
            double semiMajorRadius;
            double eccentricity;

            if (parent.exponent < 1.5d)
            {
                specificEnergy = (speed * speed) / 2d - mu;
                eccentricity = Math.Abs(Math.Sqrt(Math.Abs(((2d * specificEnergy) / mu) + 2d)) - 1d); // TODO: Incorrect for exponent 1
                //semiMajorRadius = radius * Math.Abs(mu / (2d * specificEnergy));
                semiMajorRadius = angularMomemntum / (Math.Sqrt(mu) * (1d - (eccentricity * eccentricity)));
            }
            else
            {
                //https://web.archive.org/web/20160418175843/https://ccar.colorado.edu/asen5070/handouts/cart2kep2002.pdf
                specificEnergy = (speed * speed) / 2d - (mu / radius);
                semiMajorRadius = Math.Abs(mu / (2d * specificEnergy));
                //eccentricity = Math.Sqrt(Math.Abs(1 - ((angularMomemntum * angularMomemntum) / (semiMajorRadius * mu))));
                eccentricity = Math.Sqrt(Math.Abs(1d + (2d * specificEnergy * angularMomemntum * angularMomemntum) / (mu * mu)));
            }
            var inclinationAngle = Angle.normalizeRadian(Math.Acos(orbitalMomentum.z / angularMomemntum));

            float adjustment = 0f;
            if (inclinationAngle % (Math.PI) < (Math.PI / 720f)
                || inclinationAngle % (Math.PI) > (Math.PI - (Math.PI / 720f)))
            {
                adjustment = (Math.PI / 2f) < inclinationAngle && inclinationAngle < ((3f * Math.PI) / 2f) ? 90f : -90f;
                startPosition = transform(startPosition);
                startVelocity = transform(startVelocity);
                orbitalMomentum = Vector3.Cross(startPosition, startVelocity);
                inclinationAngle = Angle.normalizeRadian(Math.Acos(Vector3.Cross(startPosition, startVelocity).z / angularMomemntum));
            }

            var ascendingAngle = Angle.normalizeRadian(Math.Atan2(orbitalMomentum.x, -1f * orbitalMomentum.y));
            var latitudeAngle = Angle.normalizeRadian(Math.Atan2(startPosition.z / Math.Sin(inclinationAngle), (startPosition.x * Math.Cos(ascendingAngle)) + (startPosition.y * Math.Sin(ascendingAngle))));
            var semiAxisRectum = Ellipse.getAxisRectum((float)semiMajorRadius, (float)eccentricity);
            double trueAnomaly;
            if (parent.exponent < 1.5d)
            {
                trueAnomaly = Angle.normalizeRadian(Math.Atan2(Math.Sqrt(1d / mu) * product, semiAxisRectum - radius));
            }
            else
            {
                trueAnomaly = Angle.normalizeRadian(Math.Atan2(Math.Sqrt(semiAxisRectum / mu) * product, semiAxisRectum - radius));
            }

            var periapseAngle = Angle.normalizeRadian(latitudeAngle - trueAnomaly);
            var kepler = KeplerCoordinates.fromTrueAnomaly((float)eccentricity, (float)semiMajorRadius, Angle.toDegrees((float)inclinationAngle), Angle.toDegrees((float)periapseAngle), Angle.toDegrees((float)ascendingAngle), Angle.toDegrees((float)trueAnomaly));
            return KeplerCoordinates.fromTimeSincePeriapsis(parent, (float)eccentricity, (float)semiMajorRadius, Angle.toDegrees((float)inclinationAngle) + adjustment, Angle.toDegrees((float)periapseAngle), Angle.toDegrees((float)ascendingAngle), kepler.getTimeSincePeriapsis(parent) - timeSinceStart);
        }

        //https://space.stackexchange.com/questions/19322/converting-orbital-elements-to-cartesian-state-vectors
        //https://web.archive.org/web/20170810015111/http://ccar.colorado.edu/asen5070/handouts/kep2cart_2002.doc
        //https://downloads.rene-schwarz.com/download/M001-Keplerian_Orbit_Elements_to_Cartesian_State_Vectors.pdf
        public static Tuple<Vector3, Vector3> toCartesian(Gravity parent, float timeSinceStart, KeplerCoordinates keplerCoordinates)
        {
            keplerCoordinates = KeplerCoordinates.fromTimeSincePeriapsis(parent, keplerCoordinates.eccentricity, keplerCoordinates.semiMajorRadius, keplerCoordinates.inclinationAngle, keplerCoordinates.periapseAngle, keplerCoordinates.ascendingAngle, keplerCoordinates.getTimeSincePeriapsis(parent) + timeSinceStart);
            return toCartesianTrueAnomaly(parent, keplerCoordinates.eccentricity, keplerCoordinates.semiMajorRadius, keplerCoordinates.inclinationAngle, keplerCoordinates.periapseAngle, keplerCoordinates.ascendingAngle, keplerCoordinates.trueAnomaly); 
        }

        public static Tuple<Vector3, Vector3> toCartesianTrueAnomaly(Gravity parent, float eccentricity, float semiMajorRadius, float inclinationAngle, float periapseAngle, float ascendingAngle, float trueAnomaly)
        {
            inclinationAngle = Angle.toRadian(inclinationAngle);
            periapseAngle = Angle.toRadian(periapseAngle);
            ascendingAngle = Angle.toRadian(ascendingAngle);
            trueAnomaly = Angle.toRadian(trueAnomaly);

            var semiAxisRectum = Ellipse.getAxisRectum(semiMajorRadius, eccentricity);
            var radius = semiAxisRectum / (1.0 + eccentricity * Math.Cos(trueAnomaly));

            var sinsAscend = Math.Sin(ascendingAngle); // sun O
            var cosAscend = Math.Cos(ascendingAngle); // cos O
            var sinPeriapse = Math.Sin(periapseAngle); // sin w
            var cosPeriapse = Math.Cos(periapseAngle); // cos w
            var sinInclination = Math.Sin(inclinationAngle); // sin i
            var cosInclination = Math.Cos(inclinationAngle); // cos i

            var oX = radius * Math.Cos(trueAnomaly);
            var oY = radius * Math.Sin(trueAnomaly);

            double od;
            if (parent.exponent < 1.5f)
            {
                od = Math.Sqrt(parent.mu);
            }
            else
            {
                od = Math.Sqrt(parent.mu / (semiAxisRectum));
            }
            var odX = od * -1d * Math.Sin(trueAnomaly);
            var odY = od * (eccentricity + Math.Cos(trueAnomaly));

            var X = (oX * ((cosPeriapse * cosAscend) - (sinPeriapse * cosInclination * sinsAscend)) - oY * ((sinPeriapse * cosAscend) + (cosPeriapse * cosInclination * sinsAscend)));
            var Y = (oX * ((cosPeriapse * sinsAscend) + (sinPeriapse * cosInclination * cosAscend)) - oY * ((sinPeriapse * sinsAscend) - (cosPeriapse * cosInclination * cosAscend)));
            var Z = ((oX * sinPeriapse * sinInclination) + (oY * cosPeriapse * sinInclination));

            var dX = (odX * ((cosPeriapse * cosAscend) - (sinPeriapse * cosInclination * sinsAscend)) - odY * ((sinPeriapse * cosAscend) + (cosPeriapse * cosInclination * sinsAscend)));
            var dY = (odX * ((cosPeriapse * sinsAscend) + (sinPeriapse * cosInclination * cosAscend)) - odY * ((sinPeriapse * sinsAscend) - (cosPeriapse * cosInclination * cosAscend)));
            var dZ = ((odX * sinPeriapse * sinInclination) + (odY * cosPeriapse * sinInclination));

            return Tuple.Create(inverseTransform((float)X, (float)Y, (float)Z), inverseTransform((float)dX, (float)dY, (float)dZ));
        }

        private static Vector3 transform(Vector3 vector)
        {
            return new Vector3(vector.x, vector.z, vector.y);
        }

        private static Vector3 inverseTransform(float x, float y, float z)
        {
            return new Vector3(x, z, y);
        }

        public static Tuple<Vector3, Vector3> getPeriapsis(Gravity parent, KeplerCoordinates keplerCoordinates)
        {
            return toCartesian(parent, 0f, keplerCoordinates.getPeriapsis());
        }

        public static Tuple<Vector3, Vector3> getDecending(Gravity parent, KeplerCoordinates keplerCoordinates)
        {
            return toCartesian(parent, 0f, keplerCoordinates.getDecending());
        }

        public static Tuple<Vector3, Vector3> getSemiMinorDecending(Gravity parent, KeplerCoordinates keplerCoordinates)
        {
            return toCartesian(parent, 0f, keplerCoordinates.getSemiMinorDecending());
        }

        public static Tuple<Vector3, Vector3> getApoapsis(Gravity parent, KeplerCoordinates keplerCoordinates)
        {
            return toCartesian(parent, 0f, keplerCoordinates.getApoapsis());
        }

        public static Tuple<Vector3, Vector3> getAscending(Gravity parent, KeplerCoordinates keplerCoordinates)
        {
            return toCartesian(parent, 0f, keplerCoordinates.getAscending());
        }

        public static Tuple<Vector3, Vector3> getSemiMinorAscending(Gravity parent, KeplerCoordinates keplerCoordinates)
        {
            return toCartesian(parent, 0f, keplerCoordinates.getSemiMinorAscending());
        }
    }
}

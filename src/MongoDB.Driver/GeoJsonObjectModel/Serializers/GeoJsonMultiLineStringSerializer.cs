﻿/* Copyright 2010-2014 MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoDB.Driver.GeoJsonObjectModel.Serializers
{
    /// <summary>
    /// Represents a serializer for a GeoJsonMultiLineString value.
    /// </summary>
    /// <typeparam name="TCoordinates">The type of the coordinates.</typeparam>
    public class GeoJsonMultiLineStringSerializer<TCoordinates> : ClassSerializerBase<GeoJsonMultiLineString<TCoordinates>> where TCoordinates : GeoJsonCoordinates
    {
        // public methods
        /// <summary>
        /// Deserializes a value.
        /// </summary>
        /// <param name="context">The deserialization context.</param>
        /// <returns>The value.</returns>
        public override GeoJsonMultiLineString<TCoordinates> Deserialize(BsonDeserializationContext context)
        {
            var helper = new Helper();
            return (GeoJsonMultiLineString<TCoordinates>)helper.Deserialize(context);
        }

        /// <summary>
        /// Serializes a value.
        /// </summary>
        /// <param name="context">The serialization context.</param>
        /// <param name="value">The value.</param>
        protected override void SerializeValue(BsonSerializationContext context, GeoJsonMultiLineString<TCoordinates> value)
        {
            var helper = new Helper();
            helper.SerializeValue(context, value);
        }

        // nested classes
        private class Helper : GeoJsonGeometrySerializer<TCoordinates>.Helper
        {
            // private fields
            private readonly IBsonSerializer<GeoJsonMultiLineStringCoordinates<TCoordinates>> _coordinatesSerializer = BsonSerializer.LookupSerializer<GeoJsonMultiLineStringCoordinates<TCoordinates>>();
            private GeoJsonMultiLineStringCoordinates<TCoordinates> _coordinates;

            // constructors
            public Helper()
                : base(typeof(GeoJsonMultiLineString<TCoordinates>), "MultiLineString", new GeoJsonObjectArgs<TCoordinates>())
            {
            }

            // public properties
            public GeoJsonMultiLineStringCoordinates<TCoordinates> Coordinates
            {
                get { return _coordinates; }
                set { _coordinates = value; }
            }

            // protected methods
            protected override GeoJsonObject<TCoordinates> CreateObject()
            {
                return new GeoJsonMultiLineString<TCoordinates>(Args, _coordinates);
            }

            /// <summary>
            /// Deserializes a field.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="name">The name.</param>
            protected override void DeserializeField(BsonDeserializationContext context, string name)
            {
                switch (name)
                {
                    case "coordinates": _coordinates = DeserializeCoordinates(context); break;
                    default: base.DeserializeField(context, name); break;
                }
            }

            /// <summary>
            /// Serializes the fields.
            /// </summary>
            /// <param name="context">The context.</param>
            /// <param name="obj">The GeoJson object.</param>
            protected override void SerializeFields(BsonSerializationContext context, GeoJsonObject<TCoordinates> obj)
            {
                base.SerializeFields(context, obj);
                var multiLineString = (GeoJsonMultiLineString<TCoordinates>)obj;
                SerializeCoordinates(context, multiLineString.Coordinates);
            }

            // private methods
            private GeoJsonMultiLineStringCoordinates<TCoordinates> DeserializeCoordinates(BsonDeserializationContext context)
            {
                return context.DeserializeWithChildContext(_coordinatesSerializer);
            }

            private void SerializeCoordinates(BsonSerializationContext context, GeoJsonMultiLineStringCoordinates<TCoordinates> coordinates)
            {
                context.Writer.WriteName("coordinates");
                context.SerializeWithChildContext(_coordinatesSerializer, coordinates);
            }
        }
    }
}

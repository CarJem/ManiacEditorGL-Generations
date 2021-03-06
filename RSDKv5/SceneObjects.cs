﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RSDKv5
{
    public class SceneObject
    {
        public readonly NameIdentifier Name;
        public readonly List<AttributeInfo> Attributes = new List<AttributeInfo>();
        public List<SceneEntity> Entities = new List<SceneEntity>();


        /*public SceneObjects(NameIdentifier name, List<AttributeInfo> attributes)
        {
            this.Name = name;
            this.Attributes = attributes;
        }

        public SceneObjects(string name, List<AttributeInfo> attributes) : this(new NameIdentifier(name), attributes) { }*/

        internal SceneObject(Reader reader)
        {
            Name = new NameIdentifier(reader);
            var info = Objects.GetObjectInfo(Name);
            if (info != null) Name = info.Name;

            byte attributes_count = reader.ReadByte();
            for (int i = 1; i < attributes_count; ++i)
                Attributes.Add(new AttributeInfo(reader, info));

            ushort entities_count = reader.ReadUInt16();
            for (int i = 0; i < entities_count; ++i)
                Entities.Add(new SceneEntity(reader, this));
        }

        internal void Write(Writer writer)
        {
            Name.Write(writer);

            writer.Write((byte)(Attributes.Count + 1));
            foreach (AttributeInfo attribute in Attributes)
                attribute.Write(writer);

            writer.Write((ushort)Entities.Count);
            foreach (SceneEntity entity in Entities)
                entity.Write(writer);
        }
    }
}

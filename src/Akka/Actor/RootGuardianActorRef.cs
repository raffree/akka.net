﻿using System.Collections.Generic;

namespace Akka.Actor
{
    public class RootGuardianActorRef : LocalActorRef
    {
        private InternalActorRef _tempContainer;
        private readonly InternalActorRef _deadLetters;
        private readonly IReadOnlyDictionary<string, InternalActorRef> _extraNames; 

        public RootGuardianActorRef(ActorPath path, ActorCell context, InternalActorRef deadLetters, IReadOnlyDictionary<string, InternalActorRef> extraNames)
            : base(path, context)
        {
            _deadLetters = deadLetters;
            _extraNames = extraNames;
        }

        public override InternalActorRef Parent { get { return this; } }

        public void SetTempContainer(InternalActorRef tempContainer)
        {
            _tempContainer = tempContainer;
        }

        public override InternalActorRef GetSingleChild(string name)
        {
            switch(name)
            {
                case "temp":
                    return _tempContainer;
                case "deadLetters":
                    return _deadLetters;
                default:
                    InternalActorRef extraActorRef;
                    if(_extraNames.TryGetValue(name, out extraActorRef))
                        return extraActorRef;
                    return base.GetSingleChild(name);
            }
        }
    }
}
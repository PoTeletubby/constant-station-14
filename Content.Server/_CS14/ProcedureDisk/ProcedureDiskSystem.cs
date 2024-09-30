


using Content.Server.Access.Systems;
using Content.Server.AlertLevel;
using Content.Server.Explosion.EntitySystems;
using Content.Server.Popups;
using Content.Server.Shuttles.Systems;
using Content.Server.Station.Systems;
using Content.Shared._CS14.ProcedureDisk;
using Content.Shared.Access.Systems;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Station;
using Robust.Server.GameObjects;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using SQLitePCL;

namespace Content.Server._CS14.ProcedureDisk
{
    public sealed class ProcedureDiskSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototype = default!;
        [Dependency] private readonly StationSystem _station = default!;
        [Dependency] private readonly AlertLevelSystem _alertLevel = default!;
        [Dependency] private readonly PopupSystem _popup = default!;
        [Dependency] private readonly IdCardSystem _id = default!;
        [Dependency] private readonly AccessReaderSystem _access = default!;
        [Dependency] private readonly ExplosionSystem _explosion = default!;
        [Dependency] private readonly MapLoaderSystem _mapLoader = default!;
        [Dependency] private readonly MapSystem _map = default!;

        
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<ProcedureDiskComponent, UseInHandEvent>(OnHandUse);
        }

        private void OnHandUse(EntityUid uid, ProcedureDiskComponent pdisk, UseInHandEvent args)
        {
            var userxform = Transform(args.User);
            var station = _station.GetStationInMap(userxform.MapID);
            if (station != null)
            {
                if (_access.IsAllowed(args.User, uid))
                {
                    var alertlevel = _alertLevel.GetLevel(station.GetValueOrDefault());
                    var coords = Transform(args.User).Coordinates;
                    if (alertlevel == "green" || alertlevel == "blue")
                    {
                        _popup.PopupEntity("Cannot call ERT on " + alertlevel, args.User);
                    }
                    if (alertlevel == "yellow")
                    {
                        _popup.PopupEntity("Call sent for ERT Engineering", args.User);
                        Del(uid);
                        Spawn("Ash", coords);
                        _map.CreateMap(out var mapid);
                        var dart = _mapLoader.LoadGrid(mapid, "/Maps/Shuttles/dart.yml");
                        Spawn("RandomHumanoidSpawnerERTEngineerEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTEngineerEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTEngineerEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                    }
                    if (alertlevel == "violet")
                    {
                        _popup.PopupEntity("Call sent for ERT Medical", args.User);
                        Del(uid);
                        Spawn("Ash", coords);
                        _map.CreateMap(out var mapid);
                        var dart = _mapLoader.LoadGrid(mapid, "/Maps/Shuttles/dart.yml");
                        Spawn("RandomHumanoidSpawnerCBURNUnit", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTMedicalEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTMedicalEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerCBURNUnit", Transform(dart.GetValueOrDefault()).Coordinates);
                    }
                    if (alertlevel == "red")
                    {
                        _popup.PopupEntity("Call sent for ERT Security", args.User);
                        Del(uid);
                        Spawn("Ash", coords);
                        _map.CreateMap(out var mapid);
                        var dart = _mapLoader.LoadGrid(mapid, "/Maps/Shuttles/dart.yml");
                        Spawn("RandomHumanoidSpawnerERTLeaderEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTSecurityEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTSecurityEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTSecurityEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);

                    }
                    if (alertlevel == "gamma")
                    {
                        _popup.PopupEntity("Call sent for a full ERT", args.User);
                        Del(uid);
                        Spawn("Ash", coords);
                        _map.CreateMap(out var mapid);
                        var dart = _mapLoader.LoadGrid(mapid, "/Maps/Shuttles/dart.yml");
                        Spawn("RandomHumanoidSpawnerERTLeaderEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTSecurityEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTSecurityEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTSecurityEVALecter", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTMedicalEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTMedicalEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTEngineerEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTEngineerEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTJanitorEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                        Spawn("RandomHumanoidSpawnerERTChaplainEVA", Transform(dart.GetValueOrDefault()).Coordinates);
                    }
                    if (alertlevel == "epsilon")
                    {
                        _popup.PopupEntity("It's no use", args.User);
                        // Del(uid);
                        _explosion.QueueExplosion(uid, ExplosionSystem.DefaultExplosionPrototypeId, 10, 3, 5);
                        QueueDel(uid);
                    }
                }
            }
        }
    }

}

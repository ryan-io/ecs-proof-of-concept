using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace src.systems {
	public struct WalkSystemQueryComponent : IComponentData {
		public EntityQuery WalkQuery;
	}

	public struct RunSystemQueryComponent : IComponentData {
		public EntityQuery RunQuery;
	}

	public struct JetpackSystemQueryComponent : IComponentData {
		public EntityQuery JetpackQuery;
	}

	public partial struct MoveAiSystem : ISystem {
		public void OnCreate(ref SystemState state) {
			state.RequireForUpdate<PlayerComponent>();

			EntityQueryBuilder queryBuilder = new EntityQueryBuilder(Allocator.Temp);

			BuildQueryWalk(ref state, ref queryBuilder);
			BuildQueryRun(ref state, ref queryBuilder);
			BuildQueryJetpack(ref state, ref queryBuilder);

			queryBuilder.Dispose();
		}

		static void BuildQueryWalk(ref SystemState state, ref EntityQueryBuilder builder) {
			builder = new EntityQueryBuilder(Allocator.Temp)
			         .WithAspect<MoveAspect>()
			         .WithAny<WalkParametersComponent>();

			var systemDataWalk = new WalkSystemQueryComponent {
				WalkQuery = state.GetEntityQuery(builder)
			};
			_ = state.EntityManager.AddComponentData(state.SystemHandle, systemDataWalk);

			builder.Reset();
		}

		static void BuildQueryRun(ref SystemState state, ref EntityQueryBuilder builder) {
			builder = new EntityQueryBuilder(Allocator.Temp)
			         .WithAspect<MoveAspect>()
			         .WithAny<RunParametersComponent>();

			var systemDataRun = new RunSystemQueryComponent {
				RunQuery = state.GetEntityQuery(builder)
			};
			_ = state.EntityManager.AddComponentData(state.SystemHandle, systemDataRun);

			builder.Reset();
		}

		static void BuildQueryJetpack(ref SystemState state, ref EntityQueryBuilder builder) {
			builder = new EntityQueryBuilder(Allocator.Temp)
			         .WithAspect<MoveAspect>()
			         .WithAny<JetpackParametersComponent>();

			var systemDataRun = new JetpackSystemQueryComponent {
				JetpackQuery = state.GetEntityQuery(builder)
			};
			_ = state.EntityManager.AddComponentData(state.SystemHandle, systemDataRun);

			builder.Reset();
		}

		public void OnUpdate(ref SystemState state) {
			var systemDataRun     = SystemAPI.GetComponent<RunSystemQueryComponent>(state.SystemHandle);
			var systemDataWalk    = SystemAPI.GetComponent<WalkSystemQueryComponent>(state.SystemHandle);
			var systemDataJetpack = SystemAPI.GetComponent<JetpackSystemQueryComponent>(state.SystemHandle);

			if (!systemDataJetpack.JetpackQuery.IsEmpty) {
				var job    = new MoveJob();
				var handle = job.ScheduleParallel(systemDataJetpack.JetpackQuery, state.Dependency);
				handle.Complete();
				Debug.Log("We can jetpack");
			}
			else if (!systemDataRun.RunQuery.IsEmpty) {
				var job    = new MoveJob();
				var handle = job.ScheduleParallel(systemDataRun.RunQuery, state.Dependency);
				handle.Complete();
				Debug.Log("We can run");
			}
			else if (!systemDataWalk.WalkQuery.IsEmpty) {
				var job    = new MoveJob();
				var handle = job.ScheduleParallel(systemDataWalk.WalkQuery, state.Dependency);
				handle.Complete();
				Debug.Log("We can walk");
			}
		}
	}

	public partial struct MoveJob : IJobEntity {
		public float MoveSpeed;

		public void Execute(MoveAspect move) {
			if (!move.IsInHostileRange()) {
				// stop/return to original location
				Debug.Log("Not currently in range");
			}
			else {
				// move towards player entity
				Debug.Log("In range!");
			}
		}
	}
}
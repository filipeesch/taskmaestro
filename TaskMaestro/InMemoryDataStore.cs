// namespace TaskMaestro;
//
// using System.Collections.Concurrent;
// using System.Threading.Channels;
//
// public class InMemoryDataStore : IMaestroDataStore
// {
//     private static readonly ConcurrentDictionary<Guid, ITask> Tasks = new();
//     private static readonly ConcurrentDictionary<Guid, ITask> PendingTasks = new();
//     private static readonly ConcurrentDictionary<AckCode, object> Acks = new();
//     private static readonly Channel<ITask> TaskQueue = Channel.CreateUnbounded<ITask>();
//
//     public Task SaveTasksAsync(IReadOnlyCollection<ITask> tasks, CancellationToken cancellationToken)
//     {
//         foreach (var task in tasks)
//         {
//             Tasks.TryAdd(task.Id, task);
//             PendingTasks.TryAdd(task.Id, task);
//         }
//
//         return Task.CompletedTask;
//     }
//
//     public Task SaveAcksAsync(IEnumerable<Ack> acks, CancellationToken? cancellationToken)
//     {
//         foreach (var ack in acks)
//         {
//             Acks.TryAdd(ack.Code, ack.Value);
//         }
//
//         return Task.CompletedTask;
//     }
//
//     public Task<ITask> ConsumeTaskAsync(CancellationToken token)
//     {
//         throw new NotImplementedException();
//     }
//
//     private void EvaluatePendingTasks()
//     {
//         foreach (var task in PendingTasks.Values)
//         {
//             if (!task.WaitForAcks.All(ack => Acks.ContainsKey(ack)))
//             {
//                 continue;
//             }
//         }
//     }
// }

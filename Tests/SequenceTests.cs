﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using BehaveN;

namespace Tests {
	[TestFixture]
	public class SequenceTests {

		[Test]
		public void Sequence_returns_success_with_zero_children() {
			var sequence = Sequence.Node();
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Success);
		}

		[Test]
		public void Sequence_returns_success_with_one_succeeding_child() {
			var sequence = Sequence.Node(Mocks.AlwaysSucceedsNode);
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Success);
		}

		[Test]
		public void Sequence_returns_failure_with_one_failing_child() {
			var sequence = Sequence.Node(Mocks.AlwaysFailsNode);
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Failure);
		}

		[Test]
		public void Sequence_returns_running_with_one_running_child() {
			var sequence = Sequence.Node(Mocks.AlwaysRunningNode);
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Running);
		}

		[Test]
		public void Sequence_returns_success_with_multiple_successful_nodes() {
			var callCount = new int[1];
			var counter = Mocks.SucceedCounterNode(callCount);
			var sequence = Sequence.Node(counter, 
										 counter, 
										 counter, 
										 counter, 
										 counter);
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Success);
			callCount[0].ShouldEqual(5);
		}

		[Test]
		public void Sequence_stops_when_encountering_a_running_node() {
			var callCount = new int[1];
			var counter = Mocks.SucceedCounterNode(callCount);
			var sequence = Sequence.Node(counter, 
										 counter, 
										 Mocks.AlwaysRunningNode,
										 counter);
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Running);
			callCount[0].ShouldEqual(2);
		}

		[Test]
		public void Sequence_stops_when_encountering_a_failing_node() {
			var callCount = new int[1];
			var counter = Mocks.SucceedCounterNode(callCount);
			var sequence = Sequence.Node(counter, 
										 counter, 
										 Mocks.AlwaysFailsNode,
										 counter);
			BehaviorTree.Run(sequence, Mocks.EmptyDictionary).ShouldEqual(NodeStatus.Failure);
			callCount[0].ShouldEqual(2);
		}
	}
}
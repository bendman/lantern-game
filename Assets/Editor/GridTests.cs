using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class GridTests
{
	[Test]
	public void FindCenter_OriginPerfect() {
		Vector3 position = new Vector3(0, 0, 0);

		Vector3 results = GridSystem.GetPoint(position);		
		Vector3 expected = new Vector3(0, 0, 0);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindCenter_OriginOffset() {
		Vector3 position = new Vector3(1f, 0, 1f);

		Vector3 results = GridSystem.GetPoint(position);		
		Vector3 expected = new Vector3(0, 0, 0);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindCenter_NonOriginPerfect() {
		Vector3 position = new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize);

		Vector3 results = GridSystem.GetPoint(position);		
		Vector3 expected = new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindCenter_NonOriginOffset() {
		Vector3 position = new Vector3(7f, 0, 7f);

		Vector3 results = GridSystem.GetPoint(position);		
		Vector3 expected = new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindForward_OriginPerfect() {
		Vector3 position = new Vector3(0, 0, 0);
		Vector3 forward = new Vector3(0, 0, 1f);

		Vector3 results = GridSystem.GetForwardPoint(position, forward);
		Vector3 expected = new Vector3(0, 0, GridSystem.gridSquareSize);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindForward_NonOriginPerfect() {
		Vector3 position = new Vector3(0, 0, GridSystem.gridSquareSize);
		Vector3 forward = new Vector3(0, 0, 1f);

		Vector3 results = GridSystem.GetForwardPoint(position, forward);
		Vector3 expected = new Vector3(0, 0, 12f);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindForward_OriginOffset() {
		Vector3 position = new Vector3(0, 0, 1f);
		Vector3 forward = new Vector3(0, 0, 1f);

		Vector3 results = GridSystem.GetForwardPoint(position, forward);
		Vector3 expected = new Vector3(0, 0, GridSystem.gridSquareSize);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindForward_OriginPerfectBackwards() {
		Vector3 position = new Vector3(0, 0, 0);
		Vector3 forward = new Vector3(0, 0, -1f);

		Vector3 results = GridSystem.GetForwardPoint(position, forward);
		Vector3 expected = new Vector3(0, 0, -GridSystem.gridSquareSize);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindForward_OriginOffsetLeft() {
		Vector3 position = new Vector3(-2f, 0, -GridSystem.gridSquareSize);
		Vector3 forward = new Vector3(-1f, 0, 0);

		Vector3 results = GridSystem.GetForwardPoint(position, forward);
		Vector3 expected = new Vector3(-GridSystem.gridSquareSize, 0, -GridSystem.gridSquareSize);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindForward_NonOriginOffsetLeft() {
		Vector3 position = new Vector3(-GridSystem.gridSquareSize * 2/3, 0, 0);
		Vector3 forward = new Vector3(-1f, 0, 0);

		Vector3 results = GridSystem.GetForwardPoint(position, forward);
		Vector3 expected = new Vector3(-GridSystem.gridSquareSize, 0, 0);
		Assert.AreEqual(expected, results);
	}

	[Test]
	public void FindBeyondTarget_OriginPerfect() {
		Vector3 targetPosition = new Vector3(0, 0, GridSystem.gridSquareSize);
		Vector3 forward = new Vector3(0, 0, 1f);

		Assert.AreEqual(
			false,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(0, 0, 0)),
			"not beyond target"
		);

		Assert.AreEqual(
			true,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(0, 0, GridSystem.gridSquareSize * 1.1f)),
			"beyond target"
		);
	}

	[Test]
	public void FindBeyondTarget_NonOriginPerfect() {
		Vector3 targetPosition = new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize * 2);
		Vector3 forward = new Vector3(0, 0, 1f);

		Assert.AreEqual(
			false,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize)),
			"not beyond target"
		);

		Assert.AreEqual(
			true,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize * 2.1f)),
			"beyond target"
		);
	}

	[Test]
	public void FindBeyondTarget_NonOriginOffset() {
		Vector3 targetPosition = new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize * 2);
		Vector3 forward = new Vector3(0, 0, 1f);

		Assert.AreEqual(
			false,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize * 1.1f)),
			"not beyond target"
		);

		Assert.AreEqual(
			true,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize * 2.1f)),
			"beyond target"
		);
	}

	[Test]
	public void FindBeyondTarget_NonOriginOffsetLeft() {
		Vector3 targetPosition = new Vector3(-GridSystem.gridSquareSize * 2, 0, GridSystem.gridSquareSize);
		Vector3 forward = new Vector3(-1f, 0, 0);

		Assert.AreEqual(
			false,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(GridSystem.gridSquareSize, 0, GridSystem.gridSquareSize)),
			"not beyond target"
		);

		Assert.AreEqual(
			true,
			GridSystem.IsBeyondTargetPosition(targetPosition, forward, new Vector3(-GridSystem.gridSquareSize * 2.1f, 0, GridSystem.gridSquareSize)),
			"beyond target"
		);
	}
}
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using RulesEngine.Models;
using Xunit;

namespace RulesEngine.UnitTest {
	[ExcludeFromCodeCoverage]
	public class EmptyCollectionTest {
		[Fact]
		public async Task EmptyCollectionTest_ReturnsExpectedResults() {
			var workflowName = "EmptyCollectionTestWorkflow";
			var workflow = new Workflow {
				WorkflowName = workflowName,
				Rules = new Rule[] {
										new Rule {
												RuleName = "EmptyCollectionTestRule",
												RuleExpressionType = RuleExpressionType.LambdaExpression,
												// None of the values should be 1
												Expression = "!test.intArray.Any(n => n == 1)"
										}
								}
			};
			var engine = new RulesEngine();
			engine.AddOrUpdateWorkflow(workflow);

			var staticTestData = new { intArray = new List<int> { } };

			dynamic dynamicTestData = new ExpandoObject();
			dynamicTestData.intArray = new List<int> { };

			// This works.
			var input_pass_static = new RuleParameter("test", staticTestData);
			var pass_results_static = await engine.ExecuteAllRulesAsync(workflowName, input_pass_static);
			Assert.True(pass_results_static.First().IsSuccess);

			// This doesn't.
			var input_pass_dynamic = new RuleParameter("test", dynamicTestData);
			var pass_results_dynamic = await engine.ExecuteAllRulesAsync(workflowName, input_pass_dynamic);
			Assert.True(pass_results_dynamic.First().IsSuccess);
		}
	}
}

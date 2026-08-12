const apiUrl = "http://localhost:5279/api";

async function test() {
  const loginRes2 = await fetch(`${apiUrl}/Auth/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email: "testuser2@test.com", password: "Password123!" })
  });
  const data = await loginRes2.json();
  const token = data.token;

  const payload = {
    title: "Test Task with Empty Strings",
    description: "",
    priority: 2,
    dueDate: "", // This fails! Wait, TodoService sets it to null!
    assignee: "",
    category: "",
    estimatedTime: "",
    subtasks: []
  };

  const createRes = await fetch(`${apiUrl}/Todo`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`
    },
    body: JSON.stringify(payload)
  });

  console.log("Create status:", createRes.status);
  console.log("Create response:", await createRes.text());
}

test().catch(console.error);

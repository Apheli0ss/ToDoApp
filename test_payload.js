const todo = {
  title: "test",
  description: "",
  dueDate: "",
  priority: "2",
  status: "0"
};

const payload = {
  title: todo.title,
  description: todo.description || null,
  priority: Number(todo.priority),
  dueDate: todo.dueDate || null,
  assignee: todo.assignee || null,
  category: todo.category || null,
  estimatedTime: todo.estimatedTime || null,
  subtasks: todo.subtasks || []
};

console.log(JSON.stringify(payload));

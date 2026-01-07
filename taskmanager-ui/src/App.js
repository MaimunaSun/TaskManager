import React, { useEffect, useState } from "react";
import axios from "axios";

const API_URL = "http://localhost:5279/api/tasks";

function App() {
    const [tasks, setTasks] = useState([]);
    const [filter, setFilter] = useState("all");
    const [sortBy, setSortBy] = useState("dueDate");
    const [smartView, setSmartView] = useState("all");
    const [tagFilter, setTagFilter] = useState("all");
    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [dueDate, setDueDate] = useState("");
    const [tags, setTags] = useState([]);
    const [editingTask, setEditingTask] = useState(null);

    // Fetch tasks
    const fetchTasks = () => {
        axios.get(API_URL)
            .then(res => setTasks(res.data))
            .catch(err => console.log("API fetch error:", err));
    };

    useEffect(() => {
        fetchTasks();
    }, []);

    // Priority based on due date
    const calculatePriority = (dueDate) => {
        if (!dueDate) return 3;
        const today = new Date();
        const due = new Date(dueDate);
        const diffDays = Math.ceil((due - today) / (1000 * 60 * 60 * 24));
        if (diffDays <= 1) return 1; // High
        if (diffDays <= 3) return 2; // Medium
        return 3; // Low
    };

    // Add or update task
    const handleSubmit = (e) => {
        e.preventDefault();
        const taskData = { title, description, dueDate, tags, isCompleted: false };

        if (editingTask) {
            axios.put(`${API_URL}/${editingTask.id}`, { ...editingTask, ...taskData })
                .then(() => {
                    resetForm();
                    fetchTasks();
                })
                .catch(err => console.log(err));
        } else {
            axios.post(API_URL, taskData)
                .then(() => {
                    resetForm();
                    fetchTasks();
                })
                .catch(err => console.log(err));
        }
    };

    const resetForm = () => {
        setTitle("");
        setDescription("");
        setDueDate("");
        setTags([]);
        setEditingTask(null);
    };

    // Delete task
    const handleDelete = (taskId) => {
        axios.delete(`${API_URL}/${taskId}`)
            .then(fetchTasks)
            .catch(err => console.log(err));
    };

    // Start editing
    const handleEdit = (task) => {
        setEditingTask(task);
        setTitle(task.title);
        setDescription(task.description);
        setDueDate(task.dueDate.split("T")[0]); // format YYYY-MM-DD
        setTags(task.tags || []);
    };

    // Filter and smart view
    let displayedTasks = tasks
        .filter(task => (filter === "pending" ? !task.isCompleted : filter === "completed" ? task.isCompleted : true))
        .filter(task => tagFilter === "all" || task.tags?.includes(tagFilter));

    const now = new Date();
    if (smartView === "top3Urgent") {
        displayedTasks = displayedTasks
            .sort((a, b) => calculatePriority(a.dueDate) - calculatePriority(b.dueDate))
            .slice(0, 3);
    } else if (smartView === "dueThisWeek") {
        const endOfWeek = new Date();
        endOfWeek.setDate(now.getDate() + 7);
        displayedTasks = displayedTasks.filter(task => {
            const due = new Date(task.dueDate);
            return due >= now && due <= endOfWeek;
        });
    }

    // Sorting
    displayedTasks = displayedTasks.sort((a, b) => {
        if (sortBy === "dueDate") return new Date(a.dueDate) - new Date(b.dueDate);
        if (sortBy === "createdAt") return new Date(a.createdAt) - new Date(b.createdAt);
        if (sortBy === "priority") return calculatePriority(a.dueDate) - calculatePriority(b.dueDate);
        return 0;
    });

    // Tags for filter dropdown
    const allTags = ["Work", "Personal"];

    return (
        <div style={{ padding: "20px", fontFamily: "Arial", maxWidth: "900px", margin: "0 auto" }}>
            <h1>Task Manager Dashboard</h1>

            {/* Add / Edit Task Form */}
            <form onSubmit={handleSubmit} style={{ marginBottom: "20px", border: "1px solid #ccc", padding: "15px" }}>
                <h2>{editingTask ? "Edit Task" : "Add New Task"}</h2>
                <input
                    type="text"
                    placeholder="Title"
                    value={title}
                    onChange={e => setTitle(e.target.value)}
                    required
                />
                <br /><br />
                <textarea
                    placeholder="Description"
                    value={description}
                    onChange={e => setDescription(e.target.value)}
                    required
                />
                <br /><br />
                <input
                    type="date"
                    value={dueDate}
                    onChange={e => setDueDate(e.target.value)}
                    required
                />
                <br /><br />
                {/* Fixed tag selection */}
                <label>Tag:</label>
                <select
                    value={tags[0] || ""}
                    onChange={e => setTags([e.target.value])}
                    required
                >
                    <option value="" disabled>Select tag</option>
                    <option value="Work">Work</option>
                    <option value="Personal">Personal</option>
                </select>
                <br /><br />
                <button type="submit">{editingTask ? "Update Task" : "Add Task"}</button>
                {editingTask && <button type="button" onClick={resetForm} style={{ marginLeft: "10px" }}>Cancel</button>}
            </form>

            {/* Controls */}
            <div style={{ marginBottom: "20px", display: "flex", flexWrap: "wrap", gap: "10px" }}>
                <button onClick={() => setFilter("all")}>All</button>
                <button onClick={() => setFilter("pending")}>Pending</button>
                <button onClick={() => setFilter("completed")}>Completed</button>

                <select value={sortBy} onChange={e => setSortBy(e.target.value)}>
                    <option value="dueDate">Sort by Due Date</option>
                    <option value="createdAt">Sort by Creation Date</option>
                    <option value="priority">Sort by Priority</option>
                </select>

                <select value={smartView} onChange={e => setSmartView(e.target.value)}>
                    <option value="all">All Tasks</option>
                    <option value="top3Urgent">Top 3 Urgent</option>
                    <option value="dueThisWeek">Due This Week</option>
                </select>

                <select value={tagFilter} onChange={e => setTagFilter(e.target.value)}>
                    <option value="all">All Tags</option>
                    {allTags.map(tag => <option key={tag} value={tag}>{tag}</option>)}
                </select>
            </div>

            {/* Task List */}
            {displayedTasks.length === 0 ? (
                <p>No tasks found.</p>
            ) : (
                <ul style={{ listStyle: "none", padding: 0 }}>
                    {displayedTasks.map(task => {
                        const priority = calculatePriority(task.dueDate);
                        const isOverdue = !task.isCompleted && new Date(task.dueDate) < new Date();

                        return (
                            <li key={task.id} style={{
                                padding: "10px",
                                marginBottom: "10px",
                                border: "1px solid #ccc",
                                borderLeft: `5px solid ${priority === 1 ? "red" : priority === 2 ? "orange" : "green"}`,
                                backgroundColor: isOverdue ? "#ffe6e6" : "#f9f9f9"
                            }}>
                                <strong>{task.title}</strong> – {task.description}
                                <div style={{ fontSize: "12px", marginTop: "5px" }}>
                                    Due: {new Date(task.dueDate).toLocaleDateString()} | Priority: {priority === 1 ? "High" : priority === 2 ? "Medium" : "Low"} {isOverdue && "(Overdue)"}
                                </div>
                                {task.tags && <div style={{ fontSize: "11px", marginTop: "5px" }}>
                                    Tag:
                                    <button onClick={() => setTagFilter(task.tags[0])} style={{ marginLeft: "5px", cursor: "pointer" }}>
                                        {task.tags[0]}
                                    </button>
                                </div>}
                                <div style={{ marginTop: "5px" }}>
                                    <button onClick={() => handleEdit(task)}>Edit</button>
                                    <button onClick={() => handleDelete(task.id)} style={{ marginLeft: "10px", color: "red" }}>Delete</button>
                                </div>
                            </li>
                        );
                    })}
                </ul>
            )}
        </div>
    );
}

export default App;

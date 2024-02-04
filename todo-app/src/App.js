import Header from './components/Header';
import Tasks from './components/Tasks';
import ModifyTask from './components/ModifyTask';

import { useState, useEffect } from 'react';

import Swal from "sweetalert2";
import axios from 'axios';

function App() {

    const tasksUrl = process.env.REACT_APP_TODOAPIURL;

    const [loading, setloading] = useState(true);
    const [tasks, setTasks] = useState([]); 
    const [showModifyTask, setShowModifyTask] = useState(false); 
    const [currentTask, setCurrentTask] = useState(null); 

    useEffect(() => {
        setloading(true);
        axios.get(`${tasksUrl}`)
        .then((response) => {
            setTasks(response.data);
            setloading(false);
        })
        .catch((error) => {
            setTasks([]);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'Unable to get list of tasks :('
            });
            console.error(error);
            setloading(false);
        });

    }, [])

    // Add Task
    const addTask = (task) => {
        setloading(true);
        axios.post(`${tasksUrl}`, task)
        .then((response) => {
            setTasks([...tasks, response.data]);
            setShowModifyTask(false);
            setloading(false);
            Swal.fire({
                icon: 'success',
                title: 'Yay...',
                text: 'You have successfully added a new task!'
            });
        })
        .catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Oh no...',
                text: 'Task was not added!'
            });
            console.error(error);
            setloading(false);
        });
    }

    // Delete Task
    const deleteTask = (id) => {
        const newTaskList = tasks.filter((task) => task.id !== id);
        setTasks(newTaskList);

        setloading(true);
        axios.delete(`${tasksUrl}${id}`)
        .then(() => {
            setloading(false);
            Swal.fire({
                icon: 'success',
                title: 'Cool...',
                text: 'You have successfully deleted a task!'
            });
        })
        .catch((error) => {
            Swal.fire({
                icon: 'error',
                title: 'Oh no...',
                text: 'Task was not deleted!'
            });
            console.error(error);
            setloading(false);
        });
        
    }

    const editTask = (id) => {
        const task = tasks.find((task) => task.id === id);
        setCurrentTask(task);
        setShowModifyTask(true);
    }

    const saveTask = (task) => {
        setloading(true);
        axios.put(`${tasksUrl}${task.id}`, task)
        .then((response) => {
            setTasks([...tasks.filter((t) => t.id !== task.id), task]);
            setShowModifyTask(false);
            setloading(false);
            Swal.fire({
                icon: 'success',
                title: 'Yay...',
                text: 'You have successfully saved a task!'
            });
        })
        .catch((error) => {
            setShowModifyTask(false);
            Swal.fire({
                icon: 'error',
                title: 'Oh no...',
                text: 'Task was not saved!'
            });
            console.error(error);
            setloading(false);
        });
    }

    const showAddTask = () => {
        setCurrentTask(null);
        setShowModifyTask(!showModifyTask);
    }

    return (
        <>
            {
                loading
                    ?
                    <div className="spinnerContainer">
                        <div className="spinner-grow text-primary" role="status">
                            <span className="visually-hidden">Loading...</span>
                        </div>                        
                    </div>
                    :
                    <div className="container">
                        <Header showForm={() => showAddTask()} changeTextAndColor={showModifyTask} />

                        {showModifyTask && !currentTask && <ModifyTask isNew={true} onSave={addTask} />}
                        {showModifyTask && currentTask && <ModifyTask isNew={false} task={currentTask} onSave={saveTask} />}

                        <h3>Agenda: {tasks.length}</h3>

                        {
                            tasks.length > 0
                                ?
                                (<Tasks tasks={tasks} onDelete={deleteTask} onEdit={editTask} />)
                                :
                                ('No Task Found!')
                        }
                    </div>
            }
        </>
    )
}

export default App;
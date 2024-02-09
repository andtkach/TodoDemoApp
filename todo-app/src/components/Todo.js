import Header from './Header';
import Tasks from './Tasks';
import ModifyTask from './ModifyTask';
import Settings from './Settings';
import { useState, useEffect, useContext } from 'react';
import { AuthContext } from "./AuthContext";
import { Navigate } from "react-router-dom"; 
import Swal from "sweetalert2";
import axios from 'axios';

function Todo() {

    const tasksUrl = process.env.REACT_APP_TODOAPIURL;

    const [loading, setloading] = useState(true);
    const [tasks, setTasks] = useState([]); 
    const [showModifyTask, setShowModifyTask] = useState(false); 
    const [showSettingsScreen, setShowSettingsScreen] = useState(false); 
    const [showTasksList, setShowTasksList] = useState(true); 
    const [currentTask, setCurrentTask] = useState(null); 

    const { token, loading: loadingToken } = useContext(AuthContext);
        
    useEffect(() => {
        setloading(true);
        
        if (!token) return;

        axios.get(`${tasksUrl}`, { 
            headers: {
              'Authorization': 'Bearer ' + token
            } 
          })
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

    }, [tasksUrl, token])

    if (loadingToken) {
        return null;
      }
    if (!token) {
        return <Navigate to="/login" replace />;
      }

    // Add Task
    const addTask = (task) => {
        if (!token) return;

        setloading(true);
        
        axios.post(`${tasksUrl}`, task, { 
            headers: {
              'Authorization': 'Bearer ' + token
            } 
          })
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
        if (!token) return;

        const newTaskList = tasks.filter((task) => task.id !== id);
        setTasks(newTaskList);

        setloading(true);
        axios.delete(`${tasksUrl}${id}`, { 
            headers: {
              'Authorization': 'Bearer ' + token
            } 
          })
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
        if (!token) return;

        setloading(true);
        axios.put(`${tasksUrl}${task.id}`, task, { 
            headers: {
              'Authorization': 'Bearer ' + token
            } 
          })
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

    const showSettings = () => {
        setShowSettingsScreen(!showSettingsScreen);
        setShowTasksList(!showTasksList);
    }

    const b64toBlob = (b64Data, contentType='', sliceSize=512) => {
        const byteCharacters = atob(b64Data);
        const byteArrays = [];
      
        for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
          const slice = byteCharacters.slice(offset, offset + sliceSize);
      
          const byteNumbers = new Array(slice.length);
          for (let i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
          }
      
          const byteArray = new Uint8Array(byteNumbers);
          byteArrays.push(byteArray);
        }
          
        const blob = new Blob(byteArrays, {type: contentType});
        return blob;
      }

      

    const playTask = (task) => {
        if (task && task.audio) {
            const audioStr = `data:audio/mp3;base64,${task.audio}`;
            let audioPlay = new Audio(audioStr);
            audioPlay.play();
        }
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
                        <Header showForm={() => showAddTask()} showSettings={() => showSettings()} changeTextAndColor={showModifyTask} />

                        {showModifyTask && !currentTask && <ModifyTask isNew={true} onSave={addTask} />}
                        {showModifyTask && currentTask && <ModifyTask isNew={false} task={currentTask} onSave={saveTask} />}

                        {showSettingsScreen && <Settings />}

                        {showTasksList && (
                            <div>
                                <h3>Agenda: {tasks.length}</h3>

                                {
                                    tasks.length > 0
                                        ?
                                        (<Tasks tasks={tasks} onDelete={deleteTask} onEdit={editTask} onPlay={playTask} />)
                                        :
                                        ('No Task Found!')
                                }
                            </div>
                        )}                        
                        
                    </div>
            }
        </>
    )
}

export default Todo;
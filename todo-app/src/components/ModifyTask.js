import { useEffect, useState } from 'react';
import Swal from "sweetalert2";

const ModifyTask = ({ isNew, task, onSave }) => {
    const [name, setName] = useState('');
    const [day, setDay] = useState('');
    const [id, setId] = useState(0);

    useEffect(() => {
        if (isNew) {
            setName('');
            setDay('');
            setId(0);
        } else {
            setName(task.name);
            setDay(task.day);
            setId(task.id);
        }
    }, [isNew, task, task?.id, task?.name, task?.day ])

    const onSubmit = (e) => {
        e.preventDefault();

        if (!name && !day) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Fill in your task and date or close the form!'
            })
        } else if (!name && day) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Fill in your task!'
            })
        } else if (name && !day) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Fill in your date!'
            })
        } else {
            onSave({ name, day, id });
        }

        setName('');
        setDay('');
        setId(0);
    }

    return (
        <form className="add-form" onSubmit={onSubmit}>
            <div className="form-control">
                <label>Task</label>
                <input type="text" placeholder="add task" value={name} onChange={(e) => setName(e.target.value)} />
            </div>
            <div className="form-control">
                <label>Day & Time</label>
                <input type="text" placeholder="add day & time" value={day} onChange={(e) => setDay(e.target.value)} />
            </div>

            <input type="submit" className="btn btn-block" value="Save Task" />
        </form>
    )
}

export default ModifyTask

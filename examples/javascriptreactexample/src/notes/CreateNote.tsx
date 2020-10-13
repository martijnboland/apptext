import React from 'react';
import { useForm } from 'react-hook-form';
import { Note } from './models';

interface CreateProps {
  onClose(): void,
  onCreate(note: Note): Promise<void>
}

const CreateNote = React.forwardRef<HTMLElement|null, CreateProps>(({ onClose, onCreate }, ref) => {
  const { register, handleSubmit, errors, reset } = useForm<Note>();
  const onSubmit = (note: Note): Promise<void> => {
    return onCreate(note)
      .then(() => reset());
  };

  return (
    <section className="notes-form" ref={ref}>
      <div className="heading-withbutton">
        <h2>Create note</h2>
        <div>
          <button onClick={onClose}>&times;</button>
        </div>
      </div>
      <form onSubmit={handleSubmit(onSubmit)}>
        <label htmlFor="title">Title</label>
        <input type="text" 
          name="title" 
          ref={register({
            required: { value: true, message: 'Title is required' }, 
            maxLength: { value: 50, message: 'Title may not be longer than 50 characters' }
          })}
        />
        {errors.title && <div className="form-error">{errors.title.message}</div>}
        
        <label htmlFor="content">Content</label>
        <textarea 
          name="content" 
          ref={register({
            required: { value: true, message: 'Content is required' }, 
            maxLength: { value: 500, message: 'Content may not be longer than 500 characters' }
          })}
          rows={5} 
        />
        {errors.content && <div className="form-error">{errors.content.message}</div>}

        <button type="submit">Create</button>
      </form>
    </section>
  )
});

export default CreateNote;
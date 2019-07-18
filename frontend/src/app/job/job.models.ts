import { Project } from '../project/project.models';

export enum ProcessingState
{
    Created = 0,
    InProgress = 1,
    Done = 2,
    Scheduled = 21
}

export enum Initiator
{
    Manual = 0,
    Git = 1,
    Data = 2
}

export enum FragmentType
{
    Metrics = 0,
    BuildLogs = 1,
    Model = 2
}

export interface Fragment
{
    id: string,
    created: Date,
    name: string,
    filename: string,
    type: FragmentType
}

export interface Step
{
    name: string,
    number:number,
    start:Date,
    end:Date,
    state: ProcessingState
}

export interface Job 
{
    id: string,
    created: Date,
    finished: Date, 
    state: ProcessingState,
    initiator: Initiator,
    project: Project,
    steps: Array<Step>
}
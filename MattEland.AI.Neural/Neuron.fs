namespace MattEland.AI.Neural.Functional

open System.Runtime.InteropServices

/// Represents a connection between two Neurons
type NeuronConnection(source: Neuron, ?initialWeight: decimal) =
  let mutable weight: decimal  = defaultArg initialWeight 1M ;

  /// The neuron this connection comes from
  member this.Source: Neuron = source;

  /// The mathematical weight or importance of the connection
  member this.Weight
    with get() = weight
    and set(newWeight) = weight <- newWeight

  /// Calculates the value of the connection by evaluating the weight and the current value of the source neuron
  member this.Evaluate() = this.Weight * this.Source.Value;

/// Represents a node in a Neural Network
and Neuron ([<Optional>] ?initialValue: decimal) =
  let mutable value = defaultArg initialValue 0M;
  let mutable inputs: NeuronConnection seq = Seq.empty;

  /// Exposes the current calculated amount of the Neuron
  member this.Value
    with get () = value
    and set (newValue) = value <- newValue

  /// Incoming connections from other Neurons (if any)
  member this.Inputs: NeuronConnection seq = inputs;

  /// Adds an incoming connection from another Neuron
  member this.AddIncomingConnection c = inputs <- Seq.append this.Inputs [c];

  /// Adds all connections together, stores the result in Value, and returns the value
  member this.Evaluate(): decimal =
    if not (Seq.isEmpty this.Inputs) then do
      let numInputs = Seq.length this.Inputs |> decimal
      value <- Seq.sumBy (fun (c:NeuronConnection) -> c.Evaluate()) this.Inputs / numInputs;
    value;

  /// Connects this neuron to another and returns the connection
  member this.Connect(target: Neuron) =
    let connection = new NeuronConnection(this);
    target.AddIncomingConnection(connection);
    connection;

/// A layer is just a series of Neurons in parallel that will link to every Neuron in the next layer (if any is present)
and NeuralNetLayer(numNeurons: int) =
  do if numNeurons <= 0 then invalidArg "numNeurons" "There must be at least one neuron in each layer";

  let neurons: Neuron seq = seq [ for i in 1 .. numNeurons -> new Neuron 0M]
  /// Layers should start with an empty collection of neurons
  member this.Neurons: Neuron seq = neurons;

  /// Sets the value of every neuron in the sequence to the corresponding ordered value provided
  member this.SetValues (values: decimal seq) = 
    let assignValue (n:Neuron) (v:decimal) = n.Value <- v;
    Seq.iter2 assignValue this.Neurons values

  /// Evaluates the layer and returns the value of each node
  member this.Evaluate(): decimal seq =
    for n in this.Neurons do n.Evaluate() |> ignore;
    Seq.map (fun (n:Neuron) -> n.Value) this.Neurons;

  /// Connects every node in this layer to the target layer
  member this.Connect(layer: NeuralNetLayer): unit = 
    for nSource in neurons do
      for nTarget in layer.Neurons do
        nSource.Connect(nTarget) |> ignore;

/// A high-level encapsulation of a neural net
and NeuralNet(numInputs: int, numOutputs: int) =
  do 
    if numInputs <= 0 then invalidArg "numInputs" "There must be at least one neuron in the input layer";
    if numOutputs <= 0 then invalidArg "numOutputs" "There must be at least one neuron in the output layer";

  let inputLayer: NeuralNetLayer = new NeuralNetLayer(numInputs);
  let outputLayer: NeuralNetLayer = new NeuralNetLayer(numOutputs);
  let mutable hiddenLayers: NeuralNetLayer seq = Seq.empty;
  let mutable isConnected: bool = false;

  let connectLayers (n1:NeuralNetLayer) (n2:NeuralNetLayer) = n1.Connect(n2);

  let layersMinusInput: NeuralNetLayer seq =
    seq {
      for layer in hiddenLayers do yield layer;
      yield outputLayer;
    }

  let layersMinusOutput: NeuralNetLayer seq =
    seq {
      yield inputLayer;
      for layer in hiddenLayers do yield layer;
    }

  /// Yields all connections to nodes inside of the network
  let connections = Seq.collect (fun (l:NeuralNetLayer) -> l.Neurons) layersMinusInput 
                 |> Seq.collect (fun (n:Neuron) -> n.Inputs); 

  /// Gets the layers of the neural network, in sequential order
  member this.Layers: NeuralNetLayer seq =
    seq {
      yield inputLayer;
      for layer in hiddenLayers do
        yield layer;
      yield outputLayer;
    }
        
  /// Represents the input layer for the network which take in values from another system
  member this.InputLayer = inputLayer;
  
  /// Represents the last layer in the network which has the values that will be taken out of the network
  member this.OutputLayer = outputLayer;    

  /// Connects the various layers of the neural network
  member this.Connect() =
    if isConnected then invalidOp "The Neural Network has already been connected";
    
    Seq.iter2 (fun l lNext -> connectLayers l lNext) layersMinusOutput layersMinusInput 
    isConnected <- true;
  
  /// Determines whether or not the network has been connected. After the network is connected, it can no longer be added to
  member this.IsConnected = isConnected;
  
  /// Adds a hidden layer to the middle of the neural net
  member this.AddHiddenLayer(layer: NeuralNetLayer) = 
    if isConnected then invalidOp "Hidden layers cannot be added after the network has been connected.";
    hiddenLayers <- Seq.append hiddenLayers [layer];

  /// Sets the weights on all connections in the neural network
  member this.SetWeights(weights: decimal seq) = 
    if isConnected = false then do this.Connect();
    Seq.iter2 (fun (w:decimal) (c:NeuronConnection) -> c.Weight <- w) weights connections;      

  /// Evaluates the entire neural network and yields the result of the output layer
  member this.Evaluate(): decimal seq = 
    if not isConnected then do this.Connect();

    // Iterate through the layers and run calculations
    let mutable result: decimal seq = Seq.empty;
    for layer in this.Layers do
      result <- layer.Evaluate();
    result;